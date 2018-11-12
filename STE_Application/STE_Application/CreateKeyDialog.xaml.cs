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
using System.Security.Cryptography;
using System.IO;

namespace STE_Application
{
    /// <summary>
    /// Interaction logic for CreateKeyDialog.xaml
    /// </summary>
    public partial class CreateKeyDialog : Window
    {
        public CreateKeyDialog()
        {
            InitializeComponent();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(Box1.Password==Box2.Password && Box1.Password.Length > 4)
            {
                this.OKButton.IsEnabled = true;
            }
            else
            {
                this.OKButton.IsEnabled = false;
            }
        }

        private void EncryptionStart(object sender, RoutedEventArgs e)
        {
        }
    }
}
