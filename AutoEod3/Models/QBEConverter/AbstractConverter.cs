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

    abstract class AbstractConverter
    {
        /// <summary>
        /// Тело запроса
        /// </summary>
        protected string query;

        /// <summary>
        /// Скрипт подготовленный для SQL
        /// </summary>
        protected string queryText;

        /// <summary>
        /// Список заголовков
        /// </summary>
        protected List<Header> titltes = new List<Header>();

        /// <summary>
        /// Флаг указывающий, что конвертация выполнена
        /// </summary>
        protected bool flagConvert = false;
        
        public void SetQuery(string query)
        {
            this.query = query;
        }
        
    }
}
