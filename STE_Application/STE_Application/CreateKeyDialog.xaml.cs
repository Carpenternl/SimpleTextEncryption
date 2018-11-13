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
        internal delegate void EncryptionHandler(ICryptoTransform ict, byte[] LockData);
        internal event EncryptionHandler EncryptReady;
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
            using(AesManaged CryptoKeeper = new AesManaged())
            {
                byte[] NaCl = new byte[16];
                new RNGCryptoServiceProvider().GetBytes(NaCl);
                byte[] KeyBytes = new Rfc2898DeriveBytes(Box1.Password, NaCl).GetBytes(16);
                byte[] Hash = new Rfc2898DeriveBytes(KeyBytes, NaCl,1000).GetBytes(16);
                byte[] salt_Hash = new byte[NaCl.Length + Hash.Length];
                Array.Copy(NaCl, salt_Hash, 16);
                Array.Copy(Hash, 0, salt_Hash, 16, 16);
                CryptoKeeper.Key = KeyBytes;
                CryptoKeeper.IV = NaCl;
                ICryptoTransform CryptoMonkey = CryptoKeeper.CreateEncryptor();
                EncryptReady(CryptoMonkey, salt_Hash);
                this.DialogResult = true;
            }
            
        }
    }
}
