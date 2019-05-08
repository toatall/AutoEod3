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
    /// </summary>
    class ConverterSQL: AbstractConverter, IConverter
    {
        
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ConverterSQL() { }

        /// <summary>
        /// Конструктор с именем файла
        /// </summary>
        /// <param name="query">Путь к файлу</param>
        public ConverterSQL(string query)
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
            this.queryText = this.query;
            return this.queryText;
        }

        /// <summary>
        /// Получение сконвертированного результата
        /// </summary>
        public string Body
        {
            get
            {               
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
        /// 1. Текст не начинается с двойных ковычек        
        /// </summary>
        /// <returns></returns>
        private bool checkFormat()
        {
            string body = this.query.Trim();
            
            if (body.Length <= 1)
                return false;
            
            if (body[0] == '"')
                return false;

            return true;
        }

        
    }
}
