﻿using System;
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

        GoodsPage gP;
        QrPage QrP;
        EmployeesPage eP;
        public MainWindow(OrionEntities db)
        {
            InitializeComponent();
            this.db = db;
        }

        // Обратная связь
        private void btn_Report_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(db);
            reportWindow.ShowDialog();
        }

        // Переключение окон
        private void btnGoods_Click(object sender, RoutedEventArgs e)
        {
            if(gP == null)
            {
                gP = new GoodsPage(db);
            }
            mainFrame.Navigate(gP);
        }
        private void btnEmp_Click(object sender, RoutedEventArgs e)
        {
            if (eP == null)
            {
                eP = new EmployeesPage(db);
            }
            mainFrame.Navigate(eP);
        }
        private void btnQr_Click(object sender, RoutedEventArgs e)
        {
            if(QrP == null)
            {
                QrP = new QrPage();
            } 
            mainFrame.Navigate(QrP);
        }
    }
}
