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
using System.IO;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace STE_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenPlainFile(object sender, RoutedEventArgs e)
        {
            this.Plain.Text = OpenTextFile();
            this.Secret.Text = "";
        }

        /// <summary>
        /// Let's the user open a  plaintext txt file
        /// </summary>
        private string OpenTextFile()
        {
            OpenFileDialog OpenFile = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "txt files (*.txt)|*.txt",
                DefaultExt = ".txt",
            };
            if (OpenFile.ShowDialog() == true)
            {
                string path = OpenFile.FileName;
                StreamReader Stream = new StreamReader(path);
                string s = Stream.ReadToEnd();
                Stream.Close();
                return s;
            }
            return "Failed To Open The file";
        }
        private void SaveTextFile(string text)
        {
            SaveFileDialog SaveFile = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".txt",
                Filter = "txt files (*.txt)|*.txt",
                CreatePrompt = true,
            };
            if (SaveFile.ShowDialog() == true)
            {
                string path = SaveFile.FileName;
                StreamWriter Wr = new StreamWriter(path);
                Wr.Write(text);
                Wr.Close();
            }
        }
        private void OpenSecretFile(object sender, RoutedEventArgs e)
        {
            this.Secret.Text = OpenTextFile();
            this.Plain.Text = "";
        }
        private void SavePlainFile(object sender, RoutedEventArgs e)
        {
            if (Plain.Text.Length <= 0)
            {
                MessageBox.Show("There is no text to save!");
                return;
            }

            SaveTextFile(Plain.Text);    
            
        }
        private void SaveSecretFile(object sender, RoutedEventArgs e)
        {
            if (Secret.Text.Length <= 0)
            {
                MessageBox.Show("There is no text to save!");
                return;
            }
                SaveTextFile(Secret.Text);
        }
        private void DecryptSecretFile(object sender, RoutedEventArgs e)
        {
            if (Secret.Text.Length <= 0)
            {
                MessageBox.Show("There is no text to decrypt!");
                return;
            } 
            byte[] DataPool = Convert.FromBase64String(Secret.Text);
            byte[] LockData = new byte[32];
            try
            {
                Array.Copy(DataPool, LockData, 32);
            }
            catch (Exception)
            {
                return;
                // Show an error here
            }
            LockSmithDialog lockSmithDialog = new LockSmithDialog(LockData);
            lockSmithDialog.ShowDialog();
            if(lockSmithDialog.DialogResult == true)
            {
                byte[] Data = new byte[DataPool.Length - 32];
                Array.Copy(DataPool, 32, Data, 0, Data.Length);
                ICryptoTransform ICT = lockSmithDialog.ICT;
                string mystring;
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, ICT, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            this.Plain.Text = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        private void EncryptPlainFile(object sender, RoutedEventArgs e)
        {
            if (this.Plain.Text.Length <= 0)
            {
                MessageBox.Show("No text to encrypt");
            }
            CreateKeyDialog CreateKey = new CreateKeyDialog();
            CreateKey.EncryptReady += StartEncrypt;
            CreateKey.ShowDialog();
        }
        private void StartEncrypt(ICryptoTransform ict, byte[] LockData)
        {
            byte[] encrypted;
            // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rijndaelmanaged?view=netframework-4.7.2
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, ict, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(Plain.Text);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            byte[] DataPool = new byte[encrypted.Length + LockData.Length];
            Array.Copy(LockData, DataPool, LockData.Length);
            Array.Copy(encrypted, 0, DataPool, LockData.Length, encrypted.Length);
            foreach (var item in DataPool)
            {
                Secret.Text = Convert.ToBase64String(DataPool);
            }
        }
    }
}