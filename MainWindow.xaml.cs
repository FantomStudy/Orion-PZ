using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace DB_PZ
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OrionEntities db;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new OrionEntities();
            dgGoods.ItemsSource = db.Goods.ToList();
        }
        
        //Задание номер 3
        private void dgGoods_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = dgGoods.SelectedItem as Goods;

            if (selectedItem != null)
            {
                EditWindow editWindow = new EditWindow(selectedItem, db);
                editWindow.ShowDialog();
                dgGoods.ItemsSource = db.Goods.ToList();
            }
        }

        private void Extend_Click(object sender, RoutedEventArgs e)
        {
            ExtendWindow ext = new ExtendWindow();
            ext.Show();
            this.Close();
        }
    }
}
