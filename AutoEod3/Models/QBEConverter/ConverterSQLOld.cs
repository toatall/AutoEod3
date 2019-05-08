using AutoEod3.Models.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.QBEConverter
{
    /// <summary>
    /// Конвертация из формата, который был в первых версиях АвтоЭОД
    /// Структуруа формата
    ///     "-"
    ///     "-"
    ///     "Title_1|Title_2|Title_3....|Title_N"
    ///     "SELECT ....... FROM .... WHERE ..... "
    ///     "-"
    ///     "-"
    /// Где "-" - это не нужные сейчас реквизиты
    /// Таким образом, из этого текста нужно получить заголовки и текст самого запроса
    /// В запросе также могут содержаться параметры:
    ///     %DataBase% - числовой индекс базы данных
    ///     %date_1% - дата 1
    ///     %date_2% - дата 2
    ///     %date_3% - дата 3
    ///     %date_4% - дата 4
    /// </summary>
    class ConverterSQLOld: AbstractConverter, IConverter
    {
        
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ConverterSQLOld() { }

        /// <summary>
        /// Конструктор с именем файла
        /// </summary>
        /// <param name="query">Путь к файлу</param>
        public ConverterSQLOld(string query)
        {
            //this.SetFile(query);
            this.query = query;
        }
        
        /// <summary>
        /// Проверка, является ли этот конвертатор подходящим
        /// </summary>
        public bool CheckFile
        {
            get { return this.checkFormat(); }
        }

        /// <summary>
        /// Процесс конвертации
        /// 1. Парсинг заголовков
        /// 2. Парсинг текста скрипта
        /// 3. Выполнение подстановок
        /// </summary>
        /// <returns>string</returns>
        public string Convert()
        {
            flagConvert = true;

            string[] textAll = this.query.Split('"');

            // 1            
            textAll[5].Split('|').ToList()
                .ForEach(x => this.titltes.Add(new Header(x, true)));

            // 2
            this.queryText = textAll[7];
            
            return this.queryText;
        }

        /// <summary>
        /// Получение сконвертированного результата
        /// </summary>
        public string Body
        {
            get
            {
                if (!flagConvert)
                    this.Convert();
                return this.queryText;
            }
        }

        /// <summary>
        /// Заголовки (на русском)
        /// </summary>
        public IEnumerable<Header> Titles
        {
            get { return titltes; }
        }
        
        /// <summary>
        /// Проверка файла к текущему формату
        /// Должны выполняться следующие условия
        /// 1. Текст начинается с двойных ковычек
        /// 2. Количество открывающих и закрывающих двойных ковычек - 6
        /// </summary>
        /// <returns></returns>
        private bool checkFormat()
        {
            string body = this.query.Trim();
            if (body.Length < 6)
                return false;
            // 1
            if (body[0] != '"')
                return false;

            // 2           
            if (body.Where(x => x == '"').Count() != 12)
                return false;

            return true;
        }
        
    }
}
