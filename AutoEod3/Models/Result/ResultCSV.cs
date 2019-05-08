using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoEod3.Models.DAL;
using AutoEod3.Models.Helper;

namespace AutoEod3.Models.Result
{

    /// <summary>
    /// Сохранение в файл csv
    /// </summary>
    class ResultCSV : IResult
    {

        /// <summary>
        /// Данные файла
        /// </summary>
        private List<string> dataFile = new List<string>();

        /// <summary>
        /// Список заголовков
        /// </summary>
        public List<Header> Headers { get; set; }

        /// <summary>
        /// Кодировка
        /// </summary>
        public Encoding Cahrset { get; set; }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="headers"></param>
        public ResultCSV(List<Header> headers = null)
        {
            if (headers != null)
                this.Headers = headers;

        }

        /// <summary>
        /// Добавление строки 
        /// </summary>
        /// <param name="line"></param>
        public void SaveRow(List<string> line)
        {
            string resultRow = "";
            if (line.Count == this.Headers.Count)
            {
                for (int i = 0; i < line.Count; i++)
                {
                    if (this.Headers[i].IsText)
                        resultRow += "'";

                    resultRow += line[i] + ";";                    
                }
                this.dataFile.Add(resultRow);
            }
        }

        /// <summary>
        /// Сохранение файла
        /// </summary>
        public void SaveToFile(string path)
        {
            // добавление заголовков
            this.dataFile.Insert(0, String.Join(";", this.Headers.Select(x => x.Name)));

            this.CheckPath(path);

            // запись в файл
            StreamWriter streamWriter = new StreamWriter(path, false, this.Cahrset);
            foreach (string r in this.dataFile)
            {
                streamWriter.WriteLine(r);
            }            
            streamWriter.Close();
        }


        private void CheckPath(string fullPath)
        {
            string path = Path.GetDirectoryName(fullPath);            
            if (!FileDir.ExistsDir(path))
                throw new Exception(string.Format("Не удалось создать каталог {0}", path));
        }
    }
}
