using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.Helper
{
    /// <summary>
    /// При запуске приложения
    /// </summary>
    class FileDirEvent: IEvent
    {
        
        
        

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
