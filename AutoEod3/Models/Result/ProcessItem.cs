using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.Result
{    
    /// <summary>
    /// Виды статусов
    /// </summary>
    public enum StatusType
    {
        Process,
        Cancel,
        Replace,
        Error,
        Success,
        Stop
    }

    /// <summary>
    /// Строчка в списке процессов выполнения
    /// </summary>
    public class ProcessItem: INotifyPropertyChanged
    {
        /// <summary>
        /// Отмена выполняемых скриптов
        /// </summary>
        public static bool cancel = false;

        /// <summary>
        /// Флаг, означающий, что выполняются скрипты
        /// </summary>
        public static bool running = false;

        /// <summary>
        /// Флаг, означающий завершение операции
        /// </summary>
        public static bool finish = false;
        

        /// <summary>
        /// Сброс
        /// </summary>
        public static void Reset()
        {
            cancel = false;
            running = false;
            finish = false;
            CountProcess = 0;
            CountEndProcess = 0;
            CountRunProcess = 0;

        }

        /// <summary>
        /// Общее количество процессов
        /// </summary>        
        public static int CountProcess
        {
            get; private set;            
        }

        /// <summary>
        /// Количество завершенных процессов
        /// </summary>        
        public static int CountEndProcess
        {
            get; private set;           
        }

        public static int CountRunProcess
        {
            get;
            private set;
        }

        /// <summary>
        /// Текст статуса
        /// </summary>
        private string statusText;
        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                this.statusText = value;
                this.RaisePropertyChanged("StatusText");
            }
        }

        /// <summary>
        /// Значок статуса
        /// </summary>
        private string statusIcon;
        public string StatusIcon
        {
            get
            {
                return this.statusIcon;
            }
            set
            {
                this.statusIcon = value;
                this.RaisePropertyChanged("StatusIcon");
            }
        }
        
        /// <summary>
        /// Задание статуса
        /// </summary>
        private StatusType status;
        public StatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                switch (value)
                {
                    case StatusType.Process:
                        this.StatusIcon = "Images/process.png";
                        break;
                    case StatusType.Cancel:
                        this.StatusIcon = "Images/cancel.png";
                        break;
                    case StatusType.Stop:
                        this.StatusIcon = "Images/stop.png";
                        break;
                    case StatusType.Error:
                        this.StatusIcon = "Images/error.png";
                        break;
                    case StatusType.Success:
                        this.StatusIcon = "Images/ok.png";
                        break;
                    case StatusType.Replace:
                        this.StatusIcon = "Images/pause.png";
                        break;
                }
            }
        }

        /// <summary>
        /// Флаг о наличии ошибок в ходе выполнения
        /// </summary>
        public bool IsError { get; set; }
             
        /// <summary>
        /// Операция прерывается, если = true
        /// </summary>
        private bool isCanceled = false;
        public bool IsCanceled
        {
            get
            {
                return this.isCanceled;
            }
            set
            {
                this.isCanceled = value;
                // stop process
            }
        }

        /// <summary>
        /// Идентификатор (порядковый номер)
        /// </summary>
        public string Id
        {
            get; private set;
        }

        /// <summary>
        /// Имя скрипта
        /// </summary>
        public string ScriptName
        {
            get; private set;
        }

        /// <summary>
        /// Код ИФНС
        /// </summary>
        public string CodeIfns
        {
            get; private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scriptName"></param>
        public ProcessItem(string id, string scriptName, string codeIfns)
        {
            this.Id = id;
            this.ScriptName = System.IO.Path.GetFileName(scriptName);
            this.CodeIfns = codeIfns;
            CountProcess++;            
            this.IsError = false;            
        }
        
        /// <summary>
        /// Событие об изменении свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Логика при изменении свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }        


        public void Run()
        {
            CountRunProcess++;            
        }

        /// <summary>
        /// Завершение процесса
        /// </summary>
        public void End()
        {
            CountEndProcess++;
            CountRunProcess--;
        }
    }
}
