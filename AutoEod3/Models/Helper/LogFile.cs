using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.Helper
{
    /// <summary>
    /// Логирование в файл
    /// </summary>
    class LogFile
    {

        private static readonly object lockObj = new object(); // для исключения одновременного обращения к файлу из потоков

        /// <summary>
        /// File resource
        /// </summary>
        //private static StreamWriter file;

        /// <summary>
        /// Construnctor
        /// </summary>
        /*public LogFile()
        {
            
            if (!CheckDir())
                return;
            file = File.AppendText(baseDir + "\\" + GetFileName); //new StreamWriter(baseDir + "\\" + GetFileName, true);           
        }*/

        /// <summary>
        /// 
        /// </summary>
        //public static LogFile Log = new LogFile();

        /// <summary>
        /// Базовый каталог
        /// </summary>
        private static string baseDir = FileDir.basePath + "\\Logs";

        /// <summary>
        /// Проверка наличия каталога для логов
        /// </summary>
        /// <returns></returns>
        private static bool CheckDir()
        {
            return FileDir.ExistsDir(baseDir);
        }

        /// <summary>
        /// Генерирование имени фала для лога на основе даты
        /// </summary>
        private static string GetFileName
        {
            get
            {
                return "Log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
            }
        }

        /// <summary>
        /// Дата и время (для записи в лог)
        /// </summary>
        private static string GetDate
        {
            get
            {
                return "[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] ";
            }
        }

        /// <summary>
        /// Запись строки в лог
        /// </summary>
        /// <param name="text"></param>
        /// <param name="includeDate"></param>
        public static void WriteLog(string text, bool includeDate = true)
        {
            if (CheckDir())
            {                
                lock (lockObj)
                {
                    using (StreamWriter writer = File.AppendText(baseDir + "\\" + GetFileName))
                    {
                        writer.WriteLine((includeDate ? GetDate : "") + text);
                    }
                }
            }
        }

        /// <summary>
        /// Запись строки-разделителя
        /// </summary>
        public static void WriteLogHr()
        {            
            WriteLog(new String('-', 32), false);
        }

        /// <summary>
        /// Запись пустой строки
        /// </summary>
        public static void WriteLogClear()
        {
            WriteLog("", false);
        }

    }
}
