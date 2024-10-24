﻿using System;
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
using System.Xml.Linq;

namespace DB_PZ
{
    /// <summary>
    /// Логика взаимодействия для RegistrWindow.xaml
    /// </summary>
    public partial class RegistrWindow : Window
    {
        OrionEntities db;
        public RegistrWindow(OrionEntities db)
        {
            InitializeComponent();
            this.db = db;
        }

        private void CreateAcc_Button_Click(object sender, RoutedEventArgs e)
        {
            if(tbLogin.Text != "" && tbPass.Text != "")
            {
                string salt = PasswordHelper.GenerateSalt();
                Accounts acc = new Accounts
                {
                    login = tbLogin.Text,
                    salt = salt,
                    password = PasswordHelper.HashPassword(tbPass.Text, salt)
                };
                try
                {
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                    AuthWindow authWindow = new AuthWindow();
                    authWindow.Show();
                    this.Close();
                }
                catch (Exception er)
                {
                    MessageBox.Show($"{er}", "Ошибка");
                }
            }

        }

        private void ToLogin_button_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
        }
    }
}
