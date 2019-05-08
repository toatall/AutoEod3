using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.Helper
{
    class Events
    {
                
        private static List<IEvent> eventList = new List<IEvent>();

        public static void AddEvent(IEvent e)
        {
            eventList.Add(e);
        }

        public static void RemoteEvent(IEvent e)
        {
            eventList.Remove(e);
        }

        public static void RemoveEventAt(int index)
        {
            eventList.RemoveAt(index);
        }

        public static bool WriteEvent(string title, string message, TypeLog typeLog = TypeLog.Info)
        {
            bool flag = true;
            foreach (IEvent e in eventList)
            {
                if (!e.WriteEvent(title, message, typeLog))
                    flag = false;
            }
            return flag;
        }

        public static bool WriteEventClear()
        {
            bool flag = true;
            foreach (IEvent e in eventList)
            {
                if (!e.WriteEventClear())
                    flag = false;
            }
            return flag;
        }
    }
}
