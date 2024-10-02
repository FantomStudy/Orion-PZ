using System;
using System.Collections.Generic;
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

namespace DB_PZ
{
    /// <summary>
    /// Логика взаимодействия для ExtendWindow.xaml
    /// </summary>
    public partial class ExtendWindow : Window
    {
        OrionEntities db;
        public ExtendWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new OrionEntities();
            dgGoods.ItemsSource = db.Goods
                .Join(db.Storages, g => g.storage_id, s => s.storage_id, (g, s) => new { g.article, g.name, g.cost, g.count_storage, g.image_path, g.storage_id, AddressStorage = s.address, PurposeStorage = s.purpose, g.supplier_id })
                .Join(db.Suppliers, g => g.supplier_id, sup => sup.supplier_id, (g, sup) => new { g.article, g.name, g.cost, g.count_storage, g.image_path, g.storage_id, g.AddressStorage, g.PurposeStorage, SupplierName = sup.name })
            .ToList();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
