using AutoEod3.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.QBEConverter
{
    class ShareConverter
    {

        List<Header> titles = new List<Header>();

        public List<Header> Titles
        {           
            get
            {
                return this.titles;
            }
        }

        public string Query
        {
            get; private set;
        }

        public string CurrentConverter
        {
            get; private set;
        }

        /// <summary>
        /// Все доступные конвертеры
        /// </summary>
        private List<IConverter> converters = new List<IConverter>
        {
            new ConverterSQL(),
            new ConverterSQLOld()
        };

        /// <summary>
        /// Конструктор
        /// </summary>
        public ShareConverter() { }
        
        /// <summary>
        /// Запуск процесса
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool Run(string query)
        {
            foreach (IConverter conv in converters)
            {
                conv.SetQuery(query);
                if (conv.CheckFile)
                {
                    this.Query = conv.Convert();
                    this.titles = conv.Titles.ToList();
                    return true;
                }
            }
            return false;
        }
    }
}
