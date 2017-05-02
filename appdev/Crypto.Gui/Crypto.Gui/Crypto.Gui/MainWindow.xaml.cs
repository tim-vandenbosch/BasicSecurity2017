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
using Crypto.encrypt;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Win32;
using folder = System.Windows.Forms;
using System.Threading;

namespace Crypto.Gui
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
        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            //string signedhash = File.ReadAllText("C:\\TestCrypto\\SignedHash.txt");
            //string encryptedMessage = File.ReadAllText("C:\\TestCrypto\\encryptedMessage.txt");
            //string encryptedSKey = File.ReadAllText("C:\\TestCrypto\\encryptedKey.txt");
            //string decryptedSKey = encrypt.RSA.Decrypt(encryptedSKey);
            //File.WriteAllText(@"C:\TestCrypto\decryptedKey.txt", decryptedSKey);
            //string decryptedMessage = encrypt.DES.Decrypt(encryptedMessage, decryptedSKey);
            //File.WriteAllText(@"C:\TestCrypto\decryptedMessage.txt", decryptedMessage);


            //string message = File.ReadAllText("C:\\TestCrypto\\decryptedMessage.txt");
            //MessageBox.Show(encrypt.RSA.Verify(message, signedhash).ToString());


        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnKeyPath_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            folder.FolderBrowserDialog keyPath = new folder.FolderBrowserDialog();
            if(keyPath.ShowDialog() == folder.DialogResult.OK)
            {
                Path = keyPath.SelectedPath;
            }
            txtKeyPath.Text = Path;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if(txtKeyPath.Text != "")
            {
                String path = txtKeyPath.Text;
                encrypt.RSA.Create(path);
                pbStatus.Value = 25;
                lblStatus.Content = "Generated RSA keys";
                btnHash.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Enter a path!", "No path selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnHashPath_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            folder.FolderBrowserDialog hashPath = new folder.FolderBrowserDialog();
            if (hashPath.ShowDialog() == folder.DialogResult.OK)
            {
                Path = hashPath.SelectedPath;
            }
            txtHashPath.Text = Path;
        }

        private void btnHash_Click(object sender, RoutedEventArgs e)
        {
            if (txtKeyPath.Text != "")
            {
                if (txtHashPath.Text != "")
                {
                    String path = txtHashPath.Text;
                    String filePath = txtFileToEncryptPath.Text;
                    encrypt.RSA.Handteken(filePath, path);
                    pbStatus.Value = 50;
                    lblStatus.Content = "Created Hash";
                    btnEncrypt.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Enter a path!", "No path selected", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Enter a key path!", "No key path selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnFileToEncryptPath_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            OpenFileDialog filePath = new OpenFileDialog();
            if (filePath.ShowDialog() == true)
            {
                Path = filePath.FileName;
            }
            txtFileToEncryptPath.Text = Path;
        }

        private void BtnEncryptedFilePath_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            folder.FolderBrowserDialog encryptedFilePath = new folder.FolderBrowserDialog();
            if (encryptedFilePath.ShowDialog() == folder.DialogResult.OK)
            {
                Path = encryptedFilePath.SelectedPath;
            }
            txtEncryptedFilePath.Text = Path;
        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (txtEncryptedFilePath.Text != "" && txtEncryptedKey.Text != "")
            {
                if (radDES.IsChecked == true)
                {
                    String sKey = encrypt.DES.GenerateKey();
                    encrypt.DES.Encrypt(txtFileToEncryptPath.Text, txtEncryptedFilePath.Text, sKey);
                    pbStatus.Value = 75;
                    lblStatus.Content = "DES file encryption completed";
                    encrypt.RSA.Encrypt(txtEncryptedKey.Text, txtKeyPath.Text, sKey);
                    Thread.Sleep(1000);
                    pbStatus.Value = 100;
                    lblStatus.Content = "DES key encryption completed";
                }
                else if (radAES.IsChecked == true)
                {
                    String sKey = encrypt.AES.GenerateKey();
                    encrypt.AES.Encrypt(txtFileToEncryptPath.Text, txtEncryptedFilePath.Text, sKey);
                    pbStatus.Value = 75;
                    lblStatus.Content = "AES file encryption completed";
                    encrypt.RSA.Encrypt(txtEncryptedKey.Text, txtKeyPath.Text, sKey);
                    Thread.Sleep(1000);
                    pbStatus.Value = 100;
                    lblStatus.Content = "AES key encryption completed";
                }
                else
                {
                    MessageBox.Show("Select encryption method!", "No encryption method selected", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Paths cannot be empty!", "Paths cannot be empty!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEncryptedKey_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            folder.FolderBrowserDialog encryptedKeyPath = new folder.FolderBrowserDialog();
            if (encryptedKeyPath.ShowDialog() == folder.DialogResult.OK)
            {
                Path = encryptedKeyPath.SelectedPath;
            }
            txtEncryptedKey.Text = Path;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btndKeys_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            folder.FolderBrowserDialog dKeysPath = new folder.FolderBrowserDialog();
            if (dKeysPath.ShowDialog() == folder.DialogResult.OK)
            {
                Path = dKeysPath.SelectedPath;
            }
            txtdKeys.Text = Path;
        }

        private void btnFileToDecrypt_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            OpenFileDialog dfilePath = new OpenFileDialog();
            if (dfilePath.ShowDialog() == true)
            {
                Path = dfilePath.FileName;
            }
            txtFileToDecrypt.Text = Path;
        }

        private void btnKeyToDecrypt_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            OpenFileDialog dKeyPath = new OpenFileDialog();
            if (dKeyPath.ShowDialog() == true)
            {
                Path = dKeyPath.FileName;
            }
            txtKeyToDecrypt.Text = Path;
        }

        private void btnDecryptedFile_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            folder.FolderBrowserDialog DecryptedPath = new folder.FolderBrowserDialog();
            if (DecryptedPath.ShowDialog() == folder.DialogResult.OK)
            {
                Path = DecryptedPath.SelectedPath;
            }
            txtDecryptedFile.Text = Path;
        }

        private void btndHash_Click(object sender, RoutedEventArgs e)
        {
            String Path = "";
            OpenFileDialog dHashPath = new OpenFileDialog();
            if (dHashPath.ShowDialog() == true)
            {
                Path = dHashPath.FileName;
            }
            txtdHash.Text = Path;
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            if (txtDecryptedFile.Text != "")
            {
                MessageBox.Show(encrypt.RSA.Verify(txtDecryptedFile.Text + "\\DecryptedFile.txt", txtdHash.Text).ToString());
            }
        }

        private void btnDecrypt_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtdKeys.Text != "" && txtDecryptedFile.Text != "" && txtFileToDecrypt.Text != "" && txtKeyToDecrypt.Text != "")
            {
                if (radDES1.IsChecked == true)
                {
                    String sKey = txtKeyToDecrypt.Text;
                    String decryptedKey = encrypt.RSA.Decrypt(txtdKeys.Text, sKey);
                    encrypt.DES.Decrypt(txtFileToDecrypt.Text, txtDecryptedFile.Text, decryptedKey);

                }
                else if (radAES1.IsChecked == true)
                {
                    String sKey = txtKeyToDecrypt.Text;
                    String decryptedKey = encrypt.RSA.Decrypt(txtdKeys.Text, sKey);
                    encrypt.AES.Decrypt(txtFileToDecrypt.Text, txtDecryptedFile.Text, decryptedKey);
                }
                else
                {
                    MessageBox.Show("Select encryption method!", "No encryption method selected", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Paths cannot be empty!", "Paths cannot be empty!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
}
