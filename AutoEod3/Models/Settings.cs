using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models
{
    /// <summary>
    /// Настройки при запуске скрипта на выполнение
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {

        /// <summary>
        /// Количество попыток выполнения скрипта
        /// </summary>
        private static int repeatCount = 10;
        public static string RepeatCount
        {
            get
            {
                return repeatCount.ToString();
            }
            set
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    repeatCount = i;
                    NotifyPropertyStaticChanged("RepeatCount");
                }                
            }
        }

        /// <summary>
        /// Интервал между повторениями (минут)
        /// </summary>
        private static int repeatTimeout = 10;
        public static string RepeatTimeout
        {
            get
            {
                return repeatTimeout.ToString();
            }
            set
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    repeatTimeout = i;
                    NotifyPropertyStaticChanged("RepeatTimeout");
                }
            }
        }

        /// <summary>
        /// Количество одновременно работающих потоков
        /// </summary>
        private static int countThread = 100;
        public static string CountThread
        {
            get
            {
                return countThread.ToString();
            }
            set
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    if (i > 0)
                    {
                        countThread = i;
                        NotifyPropertyStaticChanged("CountThread");
                    }
                }
            }
        }

        
        ///  PropertyChangedEventHandler Static
        public static event PropertyChangedEventHandler PropertyStaticChanged;

        /// NotifyPropertyChanged Static
        protected static void NotifyPropertyStaticChanged(string property)
        {
            if (PropertyStaticChanged != null)
                PropertyStaticChanged(typeof(Settings), new PropertyChangedEventArgs(property));
        }




        /// <summary>
        /// Дата 1
        /// </summary>
        private static DateTime date1 = DateTime.Now;

        public static DateTime Date1
        {
            get
            {
                return date1;
            }
            set
            {
                date1 = value;
                NotifyPropertyStaticChanged("Date1");               
            }
        }

        /// <summary>
        /// Дата 2
        /// </summary>
        private static DateTime date2 = DateTime.Now;

        public static DateTime Date2
        {
            get { return date2; }
            set
            {
                date2 = value;
                NotifyPropertyStaticChanged("Date2");                
            }
        }

        /// <summary>
        /// Дата 3
        /// </summary>
        private static DateTime date3 = DateTime.Now;

        public static DateTime Date3
        {
            get { return date3; }
            set
            {
                date3 = value;
                NotifyPropertyStaticChanged("Date3");
            }
        }

        /// <summary>
        /// Дата 4
        /// </summary>
        private static DateTime date4 = DateTime.Now;

        public static DateTime Date4
        {
            get { return date4; }
            set
            {
                date4 = value;
                NotifyPropertyStaticChanged("Date4");
            }
        }

        /// <summary>
        ///  Запуск скриптов в потоке
        /// </summary>
        private bool runThread = true;

        public bool RunThread
        {
            get { return this.runThread; }
            set
            {
                this.runThread = value;
                this.NotifyPropertyChanged("RunThread");
            }
        }

        /// <summary>
        /// Флаг, означающий отложенный запуск
        /// </summary>
        private static bool delayedRun = false;
        public static bool DelayedRun
        {
            get { return delayedRun; }
            set
            {
                delayedRun = value;
                NotifyPropertyStaticChanged("DelayedRun");
            }
        }

        /// <summary>
        /// Отсрочка запуска (время)
        /// </summary>
        private static TimeSpan delayedRunTime = new TimeSpan(1,0,0);
        public static string DelayedRunTime
        {
            get { return delayedRunTime.ToString(); }
            set
            {
                if (value == null) return;
                TimeSpan t;
                if (TimeSpan.TryParse(value, out t))
                {
                    delayedRunTime = t;
                    NotifyPropertyStaticChanged("DelayedRunTime");
                }                
            }
        }

        public static TimeSpan DelayedTimeSpan
        {
            get { return delayedRunTime; }
        }

        ///  PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        /// NotifyPropertyChanged
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        
        
        /// <summary>
        /// Инициализция
        /// </summary>
        public static void Init()
        {
            date1 = Properties.Settings.Default.dateRun1 != DateTime.MinValue ? Properties.Settings.Default.dateRun1 : DateTime.Now;
            date2 = Properties.Settings.Default.dateRun2 != DateTime.MinValue ? Properties.Settings.Default.dateRun2 : DateTime.Now;
            date3 = Properties.Settings.Default.dateRun3 != DateTime.MinValue ? Properties.Settings.Default.dateRun3 : DateTime.Now;
            date4 = Properties.Settings.Default.dateRun4 != DateTime.MinValue ? Properties.Settings.Default.dateRun4 : DateTime.Now;            
            CountThread = Properties.Settings.Default.countThread;
            RepeatCount = Properties.Settings.Default.repeatCount;
            RepeatTimeout = Properties.Settings.Default.repeatTimeout;
            DelayedRunTime = Properties.Settings.Default.delayedRunTime.ToString();
        }
         

        /// <summary>
        /// Сохранение настроек
        /// </summary>
        public static void Save()
        {
            Properties.Settings.Default.dateRun1 = Date1;
            Properties.Settings.Default.dateRun2 = Date2;
            Properties.Settings.Default.dateRun3 = Date3;
            Properties.Settings.Default.dateRun4 = Date4;
            Properties.Settings.Default.countThread = Settings.CountThread;
            Properties.Settings.Default.repeatCount = Settings.RepeatCount;
            Properties.Settings.Default.repeatTimeout = Settings.RepeatTimeout;
            Properties.Settings.Default.delayedRunTime = delayedRunTime;
            Properties.Settings.Default.Save();
        }


    }
}
