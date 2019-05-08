using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoEod3.Models.Helper
{
    class DialogEvent: IEvent
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="message">Сообщение об ошибке</param>
        public static void DialogError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool WriteEvent(string title, string message, TypeLog typeLog = TypeLog.Info, bool noDialog = false)
        {
            throw new NotImplementedException();
        }

        public bool WriteEventClear()
        {
            throw new NotImplementedException();
        }
    }
}
