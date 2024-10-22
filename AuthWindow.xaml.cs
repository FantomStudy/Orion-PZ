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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    
    public partial class AuthWindow : Window
    {
        OrionEntities db;
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            db = new OrionEntities();
            try
            {
                var user = db.Accounts.Where(d => (d.login == tbLogin.Text && d.password == tbPass.Text)).FirstOrDefault();
                if (user != null)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else {
                    MessageBox.Show("Логин или пароль не верны!", "Ошибка");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
        }

        private void ToReg_button_Click(object sender, RoutedEventArgs e)
        {
            RegistrWindow registrWindow = new RegistrWindow();
            registrWindow.Show();
            this.Close();
        }
    }
}
