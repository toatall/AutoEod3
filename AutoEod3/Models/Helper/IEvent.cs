using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.Helper
{

    enum TypeLog
    {
        Error,
        Warning,
        Info,
        Clear
    }

    interface IEvent
    {
        bool WriteEvent(string title, string message, TypeLog typeLog = TypeLog.Info, bool noDialog = false);
        bool WriteEventClear();
    }
}
