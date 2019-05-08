using AutoEod3.Models.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.QBEConverter
{
    /// <summary>
    /// Выполняется конвертирование из QBE файла, 
    /// скрипта старого формата и скрита нового формата в скоипт
    /// пригодный для выполнения в MS SQL
    /// </summary>
    interface IConverter
    {
        /// <summary>
        /// Скрипт
        /// </summary>
        /// <param name="query"></param>
        void SetQuery(string query);

        /// <summary>
        /// Проверка файла на текущий формат        
        /// </summary>
        /// <returns></returns>
        bool CheckFile { get; }        

        /// <summary>
        /// Процесс конвертации
        /// Обязательно! Должны выполняться следующие условия:
        /// 1. Файл уже загружен, т.е. выполнено SetFile()
        /// 2. Выполнена проверка файла, и результат = true
        /// Если условия не выполняютс, то необходимо выдать исключение
        /// </summary>
        /// <returns></returns>
        string Convert();

        /// <summary>
        /// Получение результата
        /// Если конвертация еще не выполнялась, 
        /// то сначала запустить конвертацию
        /// </summary>
        /// <returns></returns>
        string Body { get; }

        /// <summary>
        /// Заголовки
        /// (в некоторых форматах они отдельно от скрипта)
        /// </summary>
        IEnumerable<Header> Titles { get; }

    }
}
