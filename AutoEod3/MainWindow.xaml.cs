using AutoEod3.Models.Helper;
using AutoEod3.Models.QBEConverter;
using AutoEod3.Models;
using AutoEod3.Models.Result;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using AutoEod3.Models.DAL;
using System.ComponentModel;


namespace AutoEod3
{    

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        static List<ProcessItem> processItems = new List<ProcessItem>();
        static List<Thread> listThread = new List<Thread>();

        /// <summary>
        /// Узлы для treeQBE (TreeView)
        /// </summary>
        public ObservableCollection<Node> Nodes { get; private set; }
        
        /// <summary>
        /// Подключения 
        /// todo перенести в Settings
        /// </summary>
        public static Connections connections = new Connections(FileDir.basePath + "\\Connections.xml");
        
        /// <summary>
        /// MainWindow constructor
        /// </summary>
        public MainWindow()
        {
            Nodes = new ObservableCollection<Node>();
            InitializeComponent();
            Init();
        }
        
        /// <summary>
        /// Инициализация
        /// </summary>
        void Init()
        {
            LogFile.WriteLogHr();
            LogFile.WriteLog("Инициализация...");
            LoadTree();
            Settings.Init();            
        }

        /// <summary>
        /// Формирование дерева запросов на основании папки QBE
        /// </summary>
        public void LoadTree()
        {
            LogFile.WriteLog("Загрузка списка скриптов...");
            if (!FileDir.ExistsDir(FileDir.basePath + "\\Scripts"))
            {
                LogFile.WriteLog("Каталог Scripts не существует");
                MessageBox.Show("Каталог Scripts не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                treeQBE.Items.Clear();
                DirectoryInfo root = new DirectoryInfo(FileDir.basePath + "\\Scripts\\");
                Nodes.Add(FileDir.CreateDirectoryNode(root));
                treeQBE.ItemsSource = Nodes;
                LogFile.WriteLog("Загрузка списка скриптов успешно завершена");
            }
            catch (Exception e)
            {
                LogFile.WriteLog("Ошибка загрузки списка скриптов! " + e.Message + ": " + e.StackTrace);
                MessageBox.Show("Ошибка загрузки списка скриптов! " + e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Запуск выделенного скрипта
        /// </summary>
        /// <returns></returns>
        private void RunOneQuery()
        {            
            try
            {
                Node node = treeQBE.SelectedItem as Node;
                if (ProcessItem.running || node == null || node.IsDir)
                    return;

                RunSettings runSettings = new RunSettings();                
                runSettings.Owner = this;
                if (runSettings.ShowDialog() == true)
                {
                    IEnumerable<Connection> checkConnection = connections.Variants[runSettings.listTypeConnections.SelectedIndex].Connections.Where(x => x.Check == true);
                    RunThread(new List<Node>() { node }, checkConnection, (byte)TypeResults.CSV);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLog("RunQuery.Error: " + ex.Message + "|" + ex.Source + "|" + ex.StackTrace);
            }

        }

        /// <summary>
        /// Запуск скриптов, отмеченных галочками
        /// </summary>
        private void RunCheckQuery()
        {
            if (ProcessItem.running) return;

            try
            {
                ThreadPool.SetMaxThreads(1, 1);

                IEnumerable<Node> chekedNodes = this.CheckedNodes();
                if (chekedNodes.Count() == 0)
                {
                    MessageBox.Show("Необходимо отметить запросы галочками", "Не выбраны запросы", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                RunSettings runSettings = new RunSettings();                
                runSettings.Owner = this;
                if (runSettings.ShowDialog() == true)
                {                    
                    IEnumerable<Connection> checkConnection = connections.Variants[runSettings.listTypeConnections.SelectedIndex].Connections.Where(x => x.Check == true);
                    RunThread(chekedNodes, checkConnection, (byte)TypeResults.CSV);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLog("RunCheckQuery.Error: " + ex.Message + "|" + ex.Source + "|" + ex.StackTrace);
            }

        }
        
        /// <summary>
        /// Процедура обработки скриптов
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="connections"></param>
        /// <param name="resultType"></param>
        private async void RunThread(IEnumerable<Node> nodes, IEnumerable<Connection> connections, int resultType)
        {
            ProcessItem.Reset();

            btnCancel.IsEnabled = true;
            btnRunSelect.IsEnabled = false;
            btnRunAll.IsEnabled = false;
            treeQBE.IsEnabled = false;
            
            if (Settings.DelayedRun)
            {
                waitRunGroup.Visibility = System.Windows.Visibility.Visible;
                if (!await WaitDelayedRun(Settings.DelayedTimeSpan))
                {
                    btnCancel.IsEnabled = false;
                    btnRunSelect.IsEnabled = btnRunAll.IsEnabled = treeQBE.IsEnabled = true;
                    waitRunGroup.Visibility = System.Windows.Visibility.Hidden;
                    return;
                }
                waitRunGroup.Visibility = System.Windows.Visibility.Hidden;
            }


            LogFile.WriteLog("Начало процесса выполнения скриптов...");
            
            ProcessItem.running = true;

            processItems.Clear();
            listThread.Clear();
            
            int i = 1;
            // создать объекты ProcessItem и передать их в ListView
            foreach (Node node in nodes)
            {                
                foreach (Connection con in connections)
                {                    
                    if (resultType > (int)Enum.GetValues(typeof(TypeResults)).Cast<TypeResults>().Max()
                        || resultType < (int)Enum.GetValues(typeof(TypeResults)).Cast<TypeResults>().Min())
                    {
                        resultType = 1;
                    }

                    IResult result = null;
                    if (resultType == (int)TypeResults.CSV)
                        result = new ResultCSV();
                    //if (resultType == (int)TypeResults.Excel)
                    //    result = new ResultCSV();

                    ProcessItem processItem = new ProcessItem(i.ToString(), node.FullPath, con.Description);                    
                    processItem.Status = StatusType.Process;
                    processItem.StatusText = "Запуск";
                    processItems.Add(processItem);
                    
                    Runner runner = new Runner(node.FullPath, con.ConnectionString, con.Index, result, processItem);                    
                    Thread thread = new Thread(new ThreadStart(runner.Run));
                    thread.Start();

                    listThread.Add(thread);
                    
                    i++;
                }                    
            }

            listViewResult.ItemsSource = processItems;
            ICollectionView view = CollectionViewSource.GetDefaultView(processItems);
            view.Refresh();                        
            
            await WaitRun();

            btnCancel.IsEnabled = false;
            btnRunSelect.IsEnabled = true;
            btnRunAll.IsEnabled = true;
            treeQBE.IsEnabled = true;
            LogFile.WriteLog("Окончание процесса выполнения скриптов");

        }

        private async Task<bool> WaitDelayedRun(TimeSpan t)
        {         
            Runner.delayedEnd = false;
            Runner.TimeDelayed = Settings.DelayedTimeSpan;
            TimeSpan tick = new TimeSpan(0, 0, 1);

            while (!Runner.delayedEnd)
            {
                RenderTimer();

                if (ProcessItem.cancel)
                {                    
                    return false;
                }

                if (Runner.TimeDelayed.TotalSeconds <= 0)
                {                   
                    Runner.delayedEnd = true;
                }
                else
                {
                    Runner.TimeDelayed = Runner.TimeDelayed - tick;
                }

                await Task.Delay(tick);
            }

            return true;
        }

        private void RenderTimer()
        {
            waitRunText.Text = "Осталось: " + Runner.TimeDelayed.ToString();
        }
        

        /// <summary>
        /// Ожидание выполнения заданий
        /// </summary>
        /// <returns></returns>
        private async Task<bool> WaitRun()
        {
            taskBarInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;

            while (true)
            {                
                textCountThreadAll.Text = ProcessItem.CountProcess.ToString();
                textCountThreadComplete.Text = ProcessItem.CountEndProcess.ToString();               
                taskBarInfo.ProgressValue = (double)ProcessItem.CountEndProcess / (double)ProcessItem.CountProcess;

                // если отменяется
                if (ProcessItem.cancel)
                {
                    ProcessItem.finish = true;
                    ProcessItem.running = false;
                    SQL.cancel = true;
                    taskBarInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    return true;
                }

                // если все завершено само по себе
                if (ProcessItem.CountProcess == ProcessItem.CountEndProcess)
                {
                    ProcessItem.finish = true;
                    ProcessItem.running = false;
                    taskBarInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    return true;
                }
                
                await Task.Delay(1000);
            } 
       
        }

        /// <summary>
        /// Получение списка отмеченных скриптов
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <returns></returns>
        private IEnumerable<Node> CheckedNodes(Node node = null)
        {            
            if (node != null && !node.IsDir && node.IsChecked == true)
                yield return node;

            IEnumerable<Node> childrensNode = (node == null ? treeQBE.ItemsSource.Cast<Node>() : node.Children);

            foreach (Node n in childrensNode)
            {
                foreach (Node children in CheckedNodes(n))
                {
                    yield return children;
                }
            }
        }
                
        // События treeQBE
        #region treeQBE.Events (TreeView)

        /// <summary>
        /// Событе при нажатии мышки для treeQBE (TreeView)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox currentCheckBox = (CheckBox)sender;
            CheckBoxId.checkBoxId = currentCheckBox.Uid;
        }

        /// <summary>
        /// Событие при нажатии пробела для treeQBE (TreeView)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                CheckBox currentCheckBox = (CheckBox)sender;
                CheckBoxId.checkBoxId = currentCheckBox.Uid;
            }
        }

        /// <summary>
        /// Двойной клик узлу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeQBE_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.RunOneQuery();
        }

        #endregion treeQBE.Events (TreeView)
                
        // События кнопок (Click)
        #region Button Events

        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Main.Close();
        }

        /// <summary>
        /// Запуск выделенного скрипта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunSelect_Click(object sender, RoutedEventArgs e)
        {
            this.RunOneQuery();
            
        }
        
        /// <summary>
        /// Запуск скриптов отмеченных галочками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunAll_Click(object sender, RoutedEventArgs e)
        {
            this.RunCheckQuery();
        }

        /// <summary>
        /// Отмена выполнения скриптов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ProcessItem.cancel = true;
            SQL.cancel = true;
            btnCancel.IsEnabled = false;
        }


        #endregion Button Events

        /// <summary>
        /// Выход из программы
        /// (cохранение настроек)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ProcessItem.running)
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти? В данный момент не все задания завершены.", 
                    "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Settings.Save();

            Environment.Exit(Environment.ExitCode);
        }

        /// <summary>
        /// О программе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Owner = this;
            about.ShowDialog();
        }

       
    }
}
