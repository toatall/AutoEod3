using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoEod3.Models.DAL;
using AutoEod3.Models.Helper;
using AutoEod3.Models.QBEConverter;
using AutoEod3.Models.Result;
using AutoEod3;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

namespace AutoEod3.Models
{

    /// <summary>
    /// Класс который знает обо всем
    /// </summary>
    class Runner
    {        

        /// <summary>
        /// Время для отсрочки перед запуском
        /// </summary>
        private static TimeSpan timeDelayed = new TimeSpan(0,0,0);
        public static TimeSpan TimeDelayed
        {
            get { return timeDelayed; }
            set { timeDelayed = value; }
        }        

        /// <summary>
        /// Флаг означающий используется ли отсрочка перед запуском
        /// </summary>
        public static bool delayedEnd = false;
        
        /// <summary>
        /// Полный путь к скрипту
        /// </summary>
        private string scriptName = "";
        
        /// <summary>
        /// Строка подключения
        /// </summary>
        private string connectionString = "";

        /// <summary>
        /// Индекс базы данных (для подстановки)
        /// </summary>
        private string indexDb = "";

        /// <summary>
        /// Код ИФНС
        /// </summary>
        private string codeIfns = "";

        /// <summary>
        /// Тип результата (в каком формате сохранить)
        /// </summary>
        private IResult result;
        
        /// <summary>
        /// Текст скрипта
        /// </summary>
        private string queryText = "";

        /// <summary>
        /// Текущая кодировка файла-скрипта
        /// </summary>
        private Encoding scriptEncoding;

        /// <summary>
        /// Ссылка на текущую строку процесса в списке ListView
        /// </summary>
        private ProcessItem processItem;

        /// <summary>
        /// заголовки
        /// </summary>
        private List<Header> headers = new List<Header>();
        

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="connectionString"></param>
        /// <param name="indexDb"></param>
        /// <param name="codeIfns"></param>
        /// <param name="dates"></param>
        /// <param name="result"></param>
        public Runner(string scriptName, string connectionString, string indexDb, IResult result, ProcessItem processItem)
        {
            this.scriptName = scriptName;
            this.connectionString = connectionString;
            this.indexDb = indexDb;
            this.codeIfns = processItem.CodeIfns;          
            this.result = result;
            this.processItem = processItem;
            SetText(string.Format("Создание процесса для выполнения скрипта {0} для испекции {1}", scriptName, processItem.CodeIfns));
        }

        /// <summary>
        /// Имя скрипта без расширения
        /// </summary>
        public string ScriptNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.scriptName);
            }
        }

        /// <summary>
        /// Ожидание в очереди
        /// </summary>
        private void Wait()
        {
            int countThread = int.Parse(Settings.CountThread);
            while (ProcessItem.CountRunProcess >= countThread)
            {
                if (ProcessItem.cancel)
                    break;
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Общий запуск процесса выполнения скрпта
        /// </summary>
        public void Run()
        {
            
            // ждем
            Wait();

            // если отмену нажали раньше, чем дошла очередь скрипта
           if (ProcessItem.cancel) { Cancel(processItem); return; }

            // запуск процесса
            processItem.Run();
            SetText("Запуск", StatusType.Process, string.Format("[{0}][{1}] Запуск...", processItem.CodeIfns, processItem.ScriptName));
            
            // преобразование к скрипту
            try
            {
                if (ProcessItem.cancel) { Cancel(processItem); return; }
                SetText("Подготовка скрипта", StatusType.Process, string.Format("[{0}][{1}] Подготовка скрипта...", processItem.CodeIfns, processItem.ScriptName));                    
                this.RunConvertFileScript();
                SetText("Подготовка скрипта завершена", StatusType.Process, string.Format("[{0}][{1}] Подготовка скрипта завершена", processItem.CodeIfns, processItem.ScriptName));                             
            }
            catch (Exception exception)
            {                
                SetText("", StatusType.Error, string.Format("[{0}][{1}]", processItem.CodeIfns, processItem.ScriptName), exception, "Ошибка конвертации скрипта");
                processItem.End();
                return;
            }

            // подстановки
            try
            {
                if (ProcessItem.cancel) { Cancel(processItem); return; }    
                SetText("Подстановки", StatusType.Process, string.Format("[{0}][{1}] Подстановки...", processItem.CodeIfns, processItem.ScriptName));
                this.RunReplaces();
                SetText("Подстановки завершены", StatusType.Process, string.Format("[{0}][{1}] Подстановки завершены", processItem.CodeIfns, processItem.ScriptName));
            }
            catch (Exception exception)
            {                
                SetText("", StatusType.Error, string.Format("[{0}][{1}]", processItem.CodeIfns, processItem.ScriptName), exception, "Ошибка подстановки");
                processItem.End();
                return;
            }            

            // SQL
            try
            {
                int timeSleep = 60 * 1000 * int.Parse(Settings.RepeatTimeout);
                SQL sql = null;

                // выполняем в соответствии с количеством попыток (в случае возникновения ошибок SQL)
                for (int i = 1; i <= int.Parse(Settings.RepeatCount); i++)
                {                    
                    try
                    {
                        if (ProcessItem.cancel) { Cancel(processItem); return; }

                        SetText("Выполнение скрипта...", StatusType.Process, string.Format("[{0}][{1}] Выполнение скрипта...", processItem.CodeIfns, processItem.ScriptName));
                        sql = new SQL(this.connectionString, this.indexDb);
                        sql.ColumnsHeader = this.headers;
                        
                        sql.RunQuery(this.queryText);

                        if (ProcessItem.cancel) { Cancel(processItem); return; }
                        
                        SetText("Выполнение скрипта завершено", StatusType.Process, string.Format("[{0}][{1}] Выполнение скрипта завершено", processItem.CodeIfns, processItem.ScriptName));
                        break;                        
                    }
                    catch (SqlException sqlException)
                    {
                        SetText("", StatusType.Error, string.Format("[{0}][{1}]", processItem.CodeIfns, processItem.ScriptName), sqlException, "Ошибка SQL (SqlException) ");
                    }
                    catch (Exception exception)
                    {
                        SetText("", StatusType.Error, string.Format("[{0}][{1}]", processItem.CodeIfns, processItem.ScriptName), exception, "Ошибка SQL (Exception) ");
                    }

                    if (ProcessItem.cancel) { Cancel(processItem); return; }
                    this.processItem.Status = StatusType.Replace;
                    Thread.Sleep(timeSleep);
                    
                    if (i == int.Parse(Settings.RepeatCount))
                    {                        
                        processItem.End();
                        SetText("Не удалось выполнить скрипт", StatusType.Stop, string.Format("[{0}][{1}] Не удалось выполнить скрипт после {2} попыток", 
                            processItem.CodeIfns, processItem.ScriptName, Settings.RepeatCount));
                        return;
                    }
                }

                // сохранение результата
                try
                {
                    if (sql == null)
                        throw new Exception("Не определена переменная sql!");

                    if (ProcessItem.cancel) { Cancel(processItem); return; }
                                       
                    SetText("Сохранение результата", StatusType.Process, string.Format("[{0}][{1}] Сохранение результата...", processItem.CodeIfns, processItem.ScriptName));
                    result.Cahrset = this.scriptEncoding;
                    result.Headers = sql.ColumnsHeader;
                    foreach (List<string> line in sql.Data)
                    {
                        result.SaveRow(line);
                    }

                    string fileResult = FileDir.basePath + "\\Result\\" + FileDir.DirNameDate() + "\\" + this.ScriptNameWithoutExtension + "\\" + this.codeIfns + ".csv";

                    result.SaveToFile(fileResult);

                    SetText("Результат сохранен", StatusType.Process, string.Format("[{0}][{1}] Результат сохранен в '{2}'", processItem.CodeIfns, processItem.ScriptName, fileResult));
                   
                }
                catch (Exception exception)
                {
                    SetText("", StatusType.Error, string.Format("[{0}][{1}]", processItem.CodeIfns, processItem.ScriptName), exception, "Ошибка сохранения результата");
                    processItem.End();
                    return;
                }

            }            
            catch (Exception exception)
            {
                SetText("", StatusType.Error, string.Format("[{0}][{1}]", processItem.CodeIfns, processItem.ScriptName), exception, "Общая ошибка");
                processItem.End();
                return;
            }
            
            // Если все успешно завершено
            if (!ProcessItem.cancel && !this.processItem.IsError)
            {
                this.processItem.StatusText = "Завершено";
                this.processItem.Status = StatusType.Success;
            }
            
            processItem.End();
        }

        private void Cancel(ProcessItem item)
        {
            item.Status = StatusType.Cancel;
            item.StatusText = "Отменено";
            SQL.cancel = true;
        }
        
        /// <summary>
        /// Сообщение
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        private void SetText(string message, StatusType status = StatusType.Process, string messageAll = null, Exception exception = null, string typeError = "")
        {
            if (ProcessItem.cancel && status != StatusType.Cancel)
                return;

            if (messageAll == null)
                messageAll = message;

            if (exception != null)
            {
                this.processItem.IsError = true;
                this.processItem.Status = StatusType.Error;
                this.processItem.StatusText = typeError + ". " + exception.Message;
                LogFile.WriteLog(messageAll + "; " + typeError + "; {" + exception.Message + " | " + exception.Source + " | " + exception.StackTrace + "\n" + exception.Data + "}");
            }
            else
            {
                processItem.Status = status;
                processItem.StatusText = message;
                LogFile.WriteLog(messageAll);
            }
        }


        /// <summary>
        /// Конвертирование фалйа
        /// </summary>
        private void RunConvertFileScript()
        {            
            // проверка файла
            if (!File.Exists(this.scriptName))
                throw new Exception("Файл " + this.scriptName + " не найден!");

            this.scriptEncoding = FileDir.GetFileEncoding(this.scriptName);

            // загрузить скрипт           
            StreamReader stream = new StreamReader(this.scriptName, scriptEncoding);
            this.queryText = stream.ReadToEnd();                              
            stream.Close();
            
            // конвертация
            ShareConverter converter = new ShareConverter();
            if (!converter.Run(this.queryText))
                throw new Exception("Convert: Не найден подходящий конвертер!");
            this.headers = converter.Titles;
            this.queryText = converter.Query;
        }


        /// <summary>
        /// Замена параметров скрипта (индекс БД, даты)
        /// </summary>
        private void RunReplaces()
        {
            queryText = queryText.ToUpper();   

            // индекс БД                     
            queryText = queryText.Replace("%DATABASE%", indexDb).Replace("$DATABASE$", indexDb);
                        
            // даты
            queryText = queryText.Replace("$DATE1$", Settings.Date1.ToShortDateString()).Replace("%DATE1%", Settings.Date1.ToShortDateString());
            queryText = queryText.Replace("$DATE2$", Settings.Date2.ToShortDateString()).Replace("%DATE2%", Settings.Date2.ToShortDateString());
            queryText = queryText.Replace("$DATE3$", Settings.Date3.ToShortDateString()).Replace("%DATE3%", Settings.Date3.ToShortDateString());
            queryText = queryText.Replace("$DATE4$", Settings.Date4.ToShortDateString()).Replace("%DATE4%", Settings.Date4.ToShortDateString());
        }
        
    }
}
