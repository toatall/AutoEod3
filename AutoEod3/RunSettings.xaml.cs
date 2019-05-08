using AutoEod3;
using AutoEod3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace AutoEod3
{
    /// <summary>
    /// Логика взаимодействия для RunSettings.xaml
    /// </summary>
    public partial class RunSettings : Window
    {       
        public RunSettings()
        {            
            InitializeComponent();
            listTypeConnections.ItemsSource = MainWindow.connections.Variants;
            dateRun1.SelectedDate = Settings.Date1;
        }

        /// <summary>
        /// Вызов контекстного меню для установки/снятия флажков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null)
                return;
            
            foreach (Connection connection in listConnections.ItemsSource)
            {
                connection.Check = (item.Tag.ToString() == "All" ? true : (item.Tag.ToString() == "Inverse" ? !connection.Check : false));
            }            
        }

        /// <summary>
        /// Запуск процесса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseRun_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка
            // 1.1. Должна быть выбрана хотя бы одно подключение
            if (listConnections.ItemsSource.Cast<Connection>()
                .Where(x => x.Check == true)
                .Count() == 0)
            {
                MessageBox.Show("Необходимо выбрать Инспекцию", "Не выбрана Инспекция", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
        }

        /// <summary>
        /// Закрытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
