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
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>

    public partial class EmployeesPage : Page
    {
        private OrionEntities db;
        public List<Employees> Emp { get; set; }

        public EmployeesPage(OrionEntities db)
        {
            InitializeComponent();
            this.db = db;
            dgEmp.ItemsSource = db.Employees.ToList();
            comboBind();
        }

        private void comboBind()    // Установка DataContext для всего окна
        {
            OrionEntities db = new OrionEntities();
            var item = db.Employees.ToList();
            Emp = item.OrderBy(x => x.shop_id).GroupBy(x => x.shop_id).Select(g => g.First()).ToList();
            DataContext = Emp;
        }

        private void dgEmp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = dgEmp.SelectedItem as Employees;

            if (selectedItem != null)
            {
                EditEmpWindow editWindow = new EditEmpWindow(selectedItem, db);
                editWindow.ShowDialog();
                dgEmp.ItemsSource = db.Employees.ToList();
            }
        }
        private void tbSort_TextChanged(object sender, TextChangedEventArgs e)    // Сортировка при вводе в текстбокс
        {
            dgEmp.ItemsSource = db.Employees.Where(x => x.fullName.Contains(tbSort.Text)).ToList();
        }
        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)    // Сортировка при выборе в комбобоксе
        {
            dgEmp.ItemsSource = db.Employees.Where(x => x.shop_id == cbSort.SelectedIndex+1).ToList();
        }
        private void btnSort_Cancel_Click(object sender, RoutedEventArgs e)    // Отмена сортировки и сброс состояния полей
        {
            cbSort.SelectedItem = null;
            tbSort.Text = "";
            dgEmp.ItemsSource = db.Employees.ToList();
        }
        private void btn_export_Click(object sender, RoutedEventArgs e)    // Выключение взаимодействия с таблицей, добавление новых колонн и их удаление, присоединение таблиц и диалоговое окно с печатью
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog pD = new PrintDialog();

                if (tbParamExport.Text != "" && Int32.TryParse(tbParamExport.Text, out var shop))
                {
                    if (db.Employees.Any(x => x.shop_id == shop))
                    {
                        dgEmp.ItemsSource = db.Employees
                            .Where(x => x.shop_id == shop)
                            .Join(db.Posts, em => em.post_id, p => p.post_id, (em, p) => new { em.employee_id, em.fullName, em.gender, em.dateOfBirth, em.shop_id, post = p.name, p.salary, em.passport, em.phone })
                            .Join(db.Shops, em => em.shop_id, s => s.shop_id, (em, s) => new { em.employee_id, em.fullName, em.gender, em.dateOfBirth, shop_name = s.name, em.post, em.salary, em.passport, em.phone })
                        .ToList();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудников с этого магазина нет", "Ошибка");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Код магазина не указан или указан не верно, формируется общий отчет", "Предупреждение");
                    dgEmp.ItemsSource = db.Employees
                        .Join(db.Posts, em => em.post_id, p => p.post_id, (em, p) => new { em.employee_id, em.fullName, em.gender, em.dateOfBirth, em.shop_id, post = p.name, p.salary, em.passport, em.phone })
                        .Join(db.Shops, em => em.shop_id, s => s.shop_id, (em, s) => new { em.employee_id, em.fullName, em.gender, em.dateOfBirth, shop_name = s.name, em.post, em.salary, em.passport, em.phone })
                    .ToList();
                }

                shopColumn.Binding = new Binding("shop_name") ;
                postColumn.Binding = new Binding("post");


                if (pD.ShowDialog() == true)
                {
                    pD.PrintVisual(dgEmp, "Отчет");
                }

                shopColumn.Binding = new Binding("shop_id");
                postColumn.Binding = new Binding("post_id");
            }
            finally
            {
                this.IsEnabled = true;
                dgEmp.ItemsSource = db.Employees.ToList();
            }
        }
    }
}
