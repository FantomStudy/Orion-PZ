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
            db = new OrionEntities(); 
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = db.Accounts.Where(d => (d.login == tbLogin.Text)).FirstOrDefault();
                if(user != null)
                {
                    string hash = user.password;
                    string salt = user.salt;
                    bool isValid = PasswordHelper.VerifyPassword(tbPass.Text, hash, salt);
                    if (isValid)
                    {
                        MainWindow main = new MainWindow(db);
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Пароль не верный!", "Ошибка");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином нет!", "Ошибка");
                } 
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
            
        }

        private void ToRegistr_Button_Click(object sender, RoutedEventArgs e)
        {
            RegistrWindow registrWindow = new RegistrWindow(db);
            registrWindow.Show();
            this.Close();
        }
    }
}
