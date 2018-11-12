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
            TextBlock target = sender as TextBlock;
            this.Plain.Text = OpenTextFile();
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
            
        }

        private void SavePlainFile(object sender, RoutedEventArgs e)
        {
            SaveTextFile(Plain.Text);
        }

        private void SaveSecretFile(object sender, RoutedEventArgs e)
        {

        }

        private void DecryptSecretFile(object sender, RoutedEventArgs e)
        {

        }
        private void EncryptPlainFile(object sender, RoutedEventArgs e)
        {
            CreateKeyDialog CreateKey = new CreateKeyDialog();
            CreateKey.ShowDialog();
        }
    }
}
