using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DB_PZ
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private Goods _good;
        private OrionEntities db;

        public EditWindow(Goods good, OrionEntities db)
        {
            InitializeComponent();
            if (good != null)
            {
                _good = good;
                tbArt.Text = _good.article;
                tbName.Text = _good.name;
                tbCost.Text = _good.cost.ToString();
                tbStore.Text = _good.storage_id.ToString();
                tbCount.Text = _good.count_storage.ToString();
                tbSup.Text = _good.supplier_id.ToString();
                tbImage.Text = _good.image_path;
            }
            this.db = db;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                return;
            }

            //Сохраняем данные товара
            if (_good != null & (_good.cost != 0 || _good.storage_id != 0 ||_good.count_storage != 0))
            {
                if (_good.article != tbArt.Text) 
                {
                    MessageBox.Show("Извините, артикул нельзя изменить! \nДругие изменения учтены", "Ошибка");
                }
                _good.name = tbName.Text;
                _good.cost = Convert.ToDecimal(tbCost.Text);
                _good.storage_id = Convert.ToInt32(tbStore.Text);
                _good.count_storage = Convert.ToInt32(tbCount.Text);
                _good.supplier_id = Convert.ToInt32(tbSup.Text);
                _good.image_path = tbImage.Text;

                db.SaveChanges();
            }

            else //Добавление нового товара
            {
                Goods newGood = new Goods
                {
                    article = tbArt.Text,
                    name = tbName.Text,
                    cost = Convert.ToDecimal(tbCost.Text),
                    storage_id = Convert.ToInt32(tbStore.Text),
                    count_storage = Convert.ToInt32(tbCount.Text),
                    supplier_id = Convert.ToInt32(tbSup.Text),
                    image_path = tbImage.Text
                };
               
                db.Goods.Add(newGood);
                db.SaveChanges();
            }

            Close();
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectArt = db.Goods.Where(w => w.article == _good.article).FirstOrDefault();
            if (MessageBox.Show($"Вы уверены, что хотите удалить товар {_good.name}({_good.article})?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Goods.Remove(selectArt);
                db.SaveChanges();
                Close();
            }
        }
        private bool Validate()
        {
            // Проверка, что все поля не пустые
            if (string.IsNullOrEmpty(tbArt.Text) ||
                string.IsNullOrEmpty(tbName.Text) ||
                string.IsNullOrEmpty(tbCost.Text) ||
                string.IsNullOrEmpty(tbStore.Text) ||
                string.IsNullOrEmpty(tbCount.Text) ||
                string.IsNullOrEmpty(tbSup.Text) ||
                string.IsNullOrEmpty(tbImage.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка");
                return false;
            }
            if (!decimal.TryParse(tbCost.Text, out _))
            {
                MessageBox.Show("Стоимость должна быть числовым значением (дробная часть через ',')!", "Ошибка");
                return false;
            }
            if (!Int32.TryParse(tbCount.Text, out _) || !Int32.TryParse(tbStore.Text, out _) || !Int32.TryParse(tbStore.Text, out _) || !Int32.TryParse(tbSup.Text, out _))
            {
                MessageBox.Show("Стоимость, номер склада и поставщика должны быть целочисленным значением!", "Ошибка");
                return false;
            }
            if (tbArt.Text.Length != 8)
            {
                MessageBox.Show("Длина артикула - 8 символов!", "Ошибка");
                return false;
            }
            return true;
        }

        
    }
}
