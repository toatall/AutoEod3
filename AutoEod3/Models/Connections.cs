using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutoEod3.Models
{
    


    /// <summary>
    /// Подкчлюение к серверу SQL 
    /// </summary>
    public class Connection: INotifyPropertyChanged
    {
        /// <summary>
        /// Строка соединения
        /// </summary>
        public string ConnectionString
        {
            get; private set;
        }
        
        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get; private set;
        }

        /// <summary>
        /// Текст для списка подключений
        /// </summary>
        public string FullName
        {
            get
            {
                return this.Description + " (" + this.ConnectionString + ")";
            }
        }

        /// <summary>
        /// Индекс базы данных (постфикс)
        /// </summary>
        public string Index
        {
            get; private set;
        }

        /// <summary>
        /// Флажок для checkBox в интерфейсе
        /// </summary>
        private bool? check = true;

        public bool? Check
        {
            get
            {
                return this.check;
            }
            set
            {
                this.check = value;
                this.NotifyPropertyChanged("Check");
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionString">Строка соединения</param>
        /// <param name="description">Описание</param>
        /// <param name="index">Индекс (постфикс) базы данных</param> 
        public Connection(string connectionString, string description, string index)
        {
            this.ConnectionString = connectionString;
            this.Description = description;
            this.Index = index;
        }

        ///  PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        /// NotifyPropertyChanged
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    /// <summary>
    /// Варианты подключений
    /// </summary>
    public class VariantConnections: INotifyPropertyChanged
    {
        /// <summary>
        /// Наименование варианта
        /// </summary>
        public string VariantConnectionName { get; private set; }

        /// <summary>
        /// Список подключений
        /// </summary>
        public ObservableCollection<Connection> Connections { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="variantName">Наименование варианта</param>
        public VariantConnections(string variantName)
        {
            this.VariantConnectionName = variantName;
            Connections = new ObservableCollection<Connection>();
        }
        
        /// <summary>
        /// Парсинг подключений из XML
        /// </summary>
        /// <param name="nodeList">часть XML</param>
        /// <returns></returns>
        public VariantConnections GetConnections(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                string connectionString = node.Attributes["connectionString"] != null ? node.Attributes["connectionString"].Value : "";
                string description = node.Attributes["description"] != null ? node.Attributes["description"].Value : "";
                string index = node.Attributes["index"] != null ? node.Attributes["index"].Value : "";
                Connections.Add(new Connection(connectionString, description, index));                
            }
            return this;
        }

        /// PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        /// NotifyPropertyChanged
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }        
    }

    /// <summary>
    /// Итоговый класс для работы с подключениями
    /// </summary>
    public class Connections
    {
        /// <summary>
        /// Список вариантов подключения
        /// </summary>
        private ObservableCollection<VariantConnections> variants = new ObservableCollection<VariantConnections>();

        /// <summary>
        /// Настройки (даты, галочка о запуске в потоке и т.д.)
        /// </summary>
        //private Settings settings = new Settings();

        /// <summary>
        /// Список вариантов подключения
        /// </summary>
        public ObservableCollection<VariantConnections> Variants
        {
            get { return this.variants; }
        }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileConnection"></param>
        public Connections(string fileConnection)
        {
            if (!File.Exists(fileConnection))
                throw new FileNotFoundException(string.Format("Файл {0} не найден!", fileConnection));

            XmlDocument xml = new XmlDocument();
            xml.Load(fileConnection);

            // варианты
            XmlNodeList variantConnections = xml.SelectNodes("/root/connections");
            foreach (XmlNode node in variantConnections)
            {                   
                variants.Add(
                    new VariantConnections(node.Attributes["name"].Value)
                        .GetConnections(node.ChildNodes));
            }

        }
        
        /// <summary>
        /// Настройки
        /// </summary>
        /*
        public Settings CurrentSettings
        {
            get { return this.settings; }
        }
         * */

    }

}
