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
using static System.Net.Mime.MediaTypeNames;

namespace DB_PZ
{
    /// <summary>
    /// Логика взаимодействия для EditEmpWindow.xaml
    /// </summary>
    public partial class EditEmpWindow : Window
    {
        private Employees _emp;
        private OrionEntities db;
        public EditEmpWindow(Employees emp, OrionEntities db)
        {
            InitializeComponent();
            if (emp != null)
            {
                _emp = emp;
                tbId.Text = _emp.employee_id.ToString();
                tbName.Text = _emp.fullName;
                tbGender.Text = _emp.gender;
                tbShop.Text = _emp.shop_id.ToString();
                tbPost.Text = _emp.post_id.ToString();
                tbPassport.Text = _emp.passport;
                tbPhone.Text = _emp.phone;
            }
            this.db = db;
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                return;
            }

            //Сохраняем данные сотрудника
            if (_emp != null & (_emp.shop_id != 0 || _emp.post_id != 0 || _emp.employee_id != 0))
            {
                if (_emp.employee_id != Convert.ToInt32(tbId.Text) )
                {
                    MessageBox.Show("Извините, код сотрудника нельзя изменить! \nДругие изменения учтены", "Ошибка");
                }
                _emp.fullName = tbName.Text;
                _emp.gender = tbGender.Text;
                _emp.shop_id = Convert.ToInt32(tbShop.Text);
                _emp.post_id = Convert.ToInt32(tbPost.Text);
                _emp.passport = tbPassport.Text;
                _emp.phone = tbPhone.Text;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception er)
                {
                    MessageBox.Show($"{er}", "Ошибка");
                }
            }

            else //Добавление нового сотрудника
            {
                Employees newEmp = new Employees
                {
                    employee_id = Convert.ToInt32(tbId.Text),
                    fullName = tbName.Text,
                    gender = tbGender.Text,
                    shop_id = Convert.ToInt32(tbShop.Text),
                    post_id = Convert.ToInt32(tbPost.Text),
                    passport = tbPassport.Text,
                    phone = tbPhone.Text,
                };
                try
                {
                    db.Employees.Add(newEmp);
                    db.SaveChanges();
                }
                catch (Exception er)
                {
                    MessageBox.Show($"{er}", "Ошибка");
                }
                
            }

            Close();
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectEmp = db.Employees.Where(w => w.employee_id == _emp.employee_id).FirstOrDefault();
            if (MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {_emp.fullName} из базы?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    db.Employees.Remove(selectEmp);
                    db.SaveChanges();
                }
                catch (Exception er)
                {
                    MessageBox.Show($"{er}", "Ошибка");
                }
                Close();
            }
        }
        private bool Validate()
        {
            // Проверка, что все поля не пустые
            if (string.IsNullOrEmpty(tbId.Text) ||
                string.IsNullOrEmpty(tbName.Text) ||
                string.IsNullOrEmpty(tbGender.Text) ||
                string.IsNullOrEmpty(tbShop.Text) ||
                string.IsNullOrEmpty(tbPost.Text) ||
                string.IsNullOrEmpty(tbPhone.Text) ||
                string.IsNullOrEmpty(tbPassport.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка");
                return false;
            }
            if (!Int32.TryParse(tbPost.Text, out _) || !Int32.TryParse(tbShop.Text, out _) || !Int32.TryParse(tbId.Text, out _))
            {
                MessageBox.Show("Код сотрудника, должности и магазина - численное значение!", "Ошибка");
                return false;
            }
            if (tbId.Text == "0" || tbShop.Text == "0" || tbPost.Text == "0")
            {
                MessageBox.Show("Код сотрудника, должности и магазина - отличны от 0!", "Ошибка");
                return false;
            }
            if (tbGender.Text.Length != 1)
            {
                MessageBox.Show("Укажите пол одной буквой!(m или f)", "Ошибка");
                return false;
            }
            if(tbGender.Text != "m" && tbGender.Text != "f"){
                MessageBox.Show("Укажите пол одной буквой!(m или f)", "Ошибка");
                return false;
            }
            return true;
        }
    }
}
