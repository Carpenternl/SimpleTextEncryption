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

namespace STE_Application
{
    /// <summary>
    /// Interaction logic for LockSmithDialog.xaml
    /// </summary>
    public partial class LockSmithDialog : Window
    {
        byte[] lockdata;
        internal ICryptoTransform ICT;
        public LockSmithDialog(byte[] lockData)
        {
            lockdata = lockData;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(AesManaged CryptoBoi = new AesManaged())
            {
                byte[] salt = new byte[16];
                byte[] savedhash = new byte[16];
                Array.Copy(lockdata, salt, 16);
                Array.Copy(lockdata, 16, savedhash, 0, 16);
                byte[] KeyBytes = new Rfc2898DeriveBytes(pwb.Password, salt).GetBytes(16);
                byte[] EntryHash = new Rfc2898DeriveBytes(KeyBytes, salt, 1000).GetBytes(16);
                for (int i = 0; i < savedhash.Length; i++)
                {
                    if (savedhash[i] != EntryHash[i])
                    {
                        MessageBox.Show("Invalid password.");
                        return;
                    }
                }
                ICT =  CryptoBoi.CreateDecryptor(KeyBytes, salt);
             
            }
            DialogResult = true;
        }
    }
}
