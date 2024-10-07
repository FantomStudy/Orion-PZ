using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        OrionEntities db;
        public ReportWindow()
        {
            InitializeComponent();
            db = new OrionEntities();
        }

        private void btnReport_Send_Click(object sender, RoutedEventArgs e)
        {
            if (tbEmail.Text != "" && tbDesc.Text != "")
            {
                if(Regex.IsMatch(tbEmail.Text, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
                {
                    Reports report = new Reports
                    {
                        email = tbEmail.Text,
                        descript = tbDesc.Text
                    };
                    db.Reports.Add(report);
                    db.SaveChanges(); 
                    MessageBox.Show("Спасибо за обратную связь!");
                    Close();
                }
                else {
                    MessageBox.Show("Неправильный формат email!", "Ошибка");
                } 
            }
            else {
                MessageBox.Show("Введите данные в поля!", "Ошибка");
            }
           
        }
    }
}
