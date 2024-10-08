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
using QRCoder;
using QRCoder.Xaml;

namespace DB_PZ
{
    /// <summary>
    /// Логика взаимодействия для QrPage.xaml
    /// </summary>
    public partial class QrPage : Page
    {
        private Color qrCodeColor = Colors.Black;
        public QrPage()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qrCodeGenerator.CreateQrCode(tbLink.Text, QRCodeGenerator.ECCLevel.H);
            XamlQRCode qrCode = new XamlQRCode(qRCodeData);

            string hexColor = ColorToHex(qrCodeColor);

            DrawingImage qrCodeAsXaml = qrCode.GetGraphic(20, hexColor, "#ffffff");
                imageQr.Source = qrCodeAsXaml;
            
        }
        private string ColorToHex(Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            qrCodeColor = Colors.Blue;
            btnGenerate_Click(sender, e);
        }

        private void btnColor2_Click(object sender, RoutedEventArgs e)
        {
            qrCodeColor = Colors.Black;
            btnGenerate_Click(sender, e);
        }
    }
}


