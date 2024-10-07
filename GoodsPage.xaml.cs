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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DB_PZ
{
    /// <summary>
    /// Логика взаимодействия для GoodsPage.xaml
    /// </summary>
    public partial class GoodsPage : Page
    {
        private OrionEntities db;
        public List<Goods> Good { get; set; }

        public GoodsPage(OrionEntities db)
        {
            InitializeComponent();
            this.db = db;
            dgGoods.ItemsSource = db.Goods.ToList();
            comboBind();
        }
        
        private void comboBind()    // Установка DataContext для всего окна
        {
            OrionEntities db = new OrionEntities();
            var item = db.Goods.ToList();
            Good = item.OrderBy(x => x.cost).GroupBy(x => x.cost).Select(g => g.First()).ToList();
            DataContext = Good;
        }
        
        private void dgGoods_MouseDoubleClick(object sender, MouseButtonEventArgs e)    // Изменение записей в БД через двойной клик по строке
        {
            var selectedItem = dgGoods.SelectedItem as Goods;

            if (selectedItem != null)
            {
                EditGoodsWindow editWindow = new EditGoodsWindow(selectedItem, db);
                editWindow.ShowDialog();
                dgGoods.ItemsSource = db.Goods.ToList();
                comboBind();
            }
        }

        private void tbSort_TextChanged(object sender, TextChangedEventArgs e)    // Сортировка при вводе в текстбокс
        {
            dgGoods.ItemsSource = db.Goods.Where(x => x.name.Contains(tbSort.Text)).ToList();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)    // Сортировка при выборе в комбобоксе
        {
            var cost = Convert.ToDecimal(cbSort.SelectedValue);
            dgGoods.ItemsSource = db.Goods.Where(x => x.cost >= cost).ToList();
        }
       
        private void btnSort_Cancel_Click(object sender, RoutedEventArgs e)    // Отмена сортировки и сброс состояния полей
        {
            cbSort.SelectedItem = null;
            tbSort.Text = "";
            dgGoods.ItemsSource = db.Goods.ToList();
        }


        private void btn_export_Click(object sender, RoutedEventArgs e)    // Выключение взаимодействия с таблицей, добавление новых колонн и их удаление, присоединение таблиц и диалоговое окно с печатью
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog pD = new PrintDialog();

                if (tbParamExport.Text != "" && Int32.TryParse(tbParamExport.Text, out var storage))
                {
                    if (db.Goods.Any(x => x.storage_id == storage))
                    {
                        dgGoods.ItemsSource = db.Goods
                            .Where(x => x.storage_id == storage)
                            .Join(db.Storages, g => g.storage_id, s => s.storage_id, (g, s) => new { g.article, g.name, g.cost, g.count_storage, g.image_path, g.storage_id, address_storage = s.address, g.supplier_id })
                            .Join(db.Suppliers, g => g.supplier_id, sup => sup.supplier_id, (g, sup) => new { g.article, g.name, g.cost, g.count_storage, g.image_path, g.storage_id, g.address_storage, g.supplier_id, supplier_name = sup.name })
                        .ToList();
                    }
                    else
                    {
                        MessageBox.Show("Товаров с такого склада нет", "Ошибка");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Код склада не указан или указан не верно, формируется общий отчет", "Предупреждение");
                    dgGoods.ItemsSource = db.Goods
                        .Join(db.Storages, g => g.storage_id, s => s.storage_id, (g, s) => new { g.article, g.name, g.cost, g.count_storage, g.image_path, g.storage_id, address_storage = s.address, g.supplier_id })
                        .Join(db.Suppliers, g => g.supplier_id, sup => sup.supplier_id, (g, sup) => new { g.article, g.name, g.cost, g.count_storage, g.image_path, g.storage_id, g.address_storage, g.supplier_id, supplier_name = sup.name })
                    .ToList();
                }

                DataGridTextColumn address_storage = new DataGridTextColumn();
                address_storage.Header = "address-storage";
                address_storage.Binding = new Binding("address_storage");

                DataGridTextColumn supplier_name = new DataGridTextColumn();
                supplier_name.Header = "supplier-name";
                supplier_name.Binding = new Binding("supplier_name");

                if (pD.ShowDialog() == true)
                {
                    dgGoods.Columns.Add(address_storage);
                    dgGoods.Columns.Add(supplier_name);
                    pD.PrintVisual(dgGoods, "Отчет");
                }
                dgGoods.Columns.Remove(address_storage);
                dgGoods.Columns.Remove(supplier_name);
            }
            finally
            {
                this.IsEnabled = true;
                dgGoods.ItemsSource = db.Goods.ToList();
            }
        }
    }
}
