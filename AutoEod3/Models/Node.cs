using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoEod3
{
    
    /// <summary>
    /// Структура для checkbox
    /// </summary>
    public struct CheckBoxId
    {
        public static string checkBoxId;
    }
    
    /// <summary>
    /// Узлы структуры каталога QBE
    /// </summary>
    public class Node : INotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Node()
        {
            this.id = Guid.NewGuid().ToString();                        
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isDir"></param>
        public Node(string text, bool isDir, string fullPath, Node parent = null)
            : this()
        {
            this.text = text;
            this.isDir = isDir;
            this.fullPath = fullPath;
            if (this.isDir)
            {
                this.Ico = this.IsExpanded ? "Images\\folderOpen.png" : "Images\\folder.png";
            }
            else this.Ico = "Images\\query.png";

            if (parent != null)
                this.parent.Add(parent);
        }

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        private ObservableCollection<Node> children = new ObservableCollection<Node>();

        /// <summary>
        /// Родительские узлы
        /// </summary>
        private ObservableCollection<Node> parent = new ObservableCollection<Node>();

        /// <summary>
        /// ИД узла
        /// </summary>
        private string id;

        /// <summary>
        /// Текст узла
        /// </summary>
        private string text;

        /// <summary>
        /// Состояние checkbox узла
        /// </summary>
        private bool? isChecked = true;

        /// <summary>
        /// Состояние узла (свернут/развернут)
        /// </summary>
        private bool isExpanded = true;

        /// <summary>
        /// Является ли узел каталогом
        /// </summary>
        private bool isDir;

        /// <summary>
        /// Иконка узла
        /// </summary>
        private string iconFile;

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        private string fullPath;
        
        /// <summary>
        /// Дочерние узлы (свойство)
        /// </summary>
        public ObservableCollection<Node> Children
        {
            get { return this.children; }
        }

        /// <summary>
        /// Родительские узлы (свойство)
        /// </summary>
        public ObservableCollection<Node> Parent
        {
            get { return this.parent; }
        }

        /// <summary>
        /// ИД узла (свойство)
        /// </summary>
        public string Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
            }
        }
        
        /// <summary>
        /// Является ли узел каталогом
        /// </summary>
        public bool IsDir
        {
            get
            {
                return this.isDir;
            }
        }

        /// <summary>
        /// Состояние checkbox узла (свойство)
        /// </summary>
        public bool? IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        /// <summary>
        /// Текст узла (свойство)
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                RaisePropertyChanged("Text");
            }
        }

        /// <summary>
        /// Состояние узла (свернут/развернут) (свойство)
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;                
                RaisePropertyChanged("IsExpanded");
            }
        }

        /// <summary>
        /// Является ли узел каталогом (свойство)
        /// </summary>
        public string Ico
        {
            get
            {
                return iconFile;
            }            
            private set
            {
                iconFile = value;
                RaisePropertyChanged("Ico");
            }
        }        

        /// <summary>
        /// Полный путь к файлу или каталогу
        /// </summary>
        public string FullPath
        {
            get { return this.fullPath; }
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

            int countCheck = 0;

            if (propertyName == "IsChecked")
            {
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count == 0 && this.Children.Count != 0)
                {
                    CheckedTreeParent(this.Children, this.IsChecked);
                }
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count > 0 && this.Children.Count > 0)
                {
                    CheckedTreeChildMiddle(this.Parent, this.Children, this.IsChecked);
                }
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count > 0 && this.Children.Count == 0)
                {
                    CheckedTreeChild(this.Parent, countCheck);
                }
            }
           
            if (propertyName == "IsExpanded")
            {                
                if (this.isDir)
                {
                    this.Ico = this.IsExpanded ? "Images\\folderOpen.png" : "Images\\folder.png";                    
                }
            }

        }

        /// <summary>
        /// Изменение состояния родительских и дочерних узлов
        /// </summary>
        /// <param name="itemsParent">Родительские узлы</param>
        /// <param name="itemsChild">Дочерние узлы</param>
        /// <param name="isChecked">Состояние checkbox узла</param>
        private void CheckedTreeChildMiddle(ObservableCollection<Node> itemsParent, ObservableCollection<Node> itemsChild, bool? isChecked)
        {
            int countCheck = 0;
            CheckedTreeParent(itemsChild, isChecked);
            CheckedTreeChild(itemsParent, countCheck);
        }

        /// <summary>
        /// Изменение состояния родительских узлов
        /// </summary>
        /// <param name="items">Родительский узел</param>
        /// <param name="isChecked">Состояние checkbox узла</param>
        private void CheckedTreeParent(ObservableCollection<Node> items, bool? isChecked)
        {
            foreach (Node item in items)
            {
                item.IsChecked = isChecked;
                if (item.Children.Count != 0) CheckedTreeParent(item.Children, isChecked);
            }
        }

        /// <summary>
        /// Изменение состояния дочерних узлов
        /// </summary>
        /// <param name="items">Дочерние узлы</param>
        /// <param name="countCheck">Состояние checkbox узла</param>
        private void CheckedTreeChild(ObservableCollection<Node> items, int countCheck)
        {            
            bool isNull = false;
            foreach (Node parent in items)
            {
                foreach (Node child in parent.Children)
                {
                    if (child.IsChecked == true || child.IsChecked == null)
                    {
                        countCheck++;
                        if (child.IsChecked == null)
                            isNull = true;
                    }
                }
                if (countCheck != parent.Children.Count && countCheck != 0)
                    parent.IsChecked = null;
                else if (countCheck == 0)
                    parent.IsChecked = false;
                else if (countCheck == parent.Children.Count && isNull)
                    parent.IsChecked = null;
                else if (countCheck == parent.Children.Count && !isNull)
                    parent.IsChecked = true;

                if (parent.Parent.Count != 0)
                    CheckedTreeChild(parent.Parent, 0);
            }
        }
    }


}
