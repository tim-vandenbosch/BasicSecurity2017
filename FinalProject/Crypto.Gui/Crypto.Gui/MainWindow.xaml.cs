using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using Crypto.Stenografie;

namespace Crypto.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Microsoft.Win32.OpenFileDialog browseVenster = new Microsoft.Win32.OpenFileDialog();
        private String imagePath;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {

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
                encrypt.RSA.Create(path, txtSender.Text, txtReceiver.Text);
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
                    String keyPath = txtKeyPath.Text;
                    encrypt.RSA.Handteken(filePath, path, keyPath, txtSender.Text);
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
                    encrypt.RSA.Encrypt(txtEncryptedKey.Text, txtKeyPath.Text, sKey, txtReceiver.Text);
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
                    encrypt.RSA.Encrypt(txtEncryptedKey.Text, txtKeyPath.Text, sKey, txtReceiver.Text);
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

        // Hash validation
        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            var result =
                encrypt.RSA.Verify(txtDecryptedFile.Text + "\\DecryptedFile.txt", txtdHash.Text, txtdKeys.Text,
                    txtSender.Text);
            if (txtDecryptedFile.Text != "")
            {
                if (result == true)
                {
                    MessageBox.Show("Het bestand kon gevalideerd worden!", "Succes", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("Het bestand kon niet gevalideerd worden!", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnDecrypt_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtdKeys.Text != "" && txtDecryptedFile.Text != "" && txtFileToDecrypt.Text != "" && txtKeyToDecrypt.Text != "")
            {
                if (radDES1.IsChecked == true)
                {
                    String sKey = txtKeyToDecrypt.Text;
                    String decryptedKey = encrypt.RSA.Decrypt(txtdKeys.Text, sKey, txtReceiver.Text);
                    encrypt.DES.Decrypt(txtFileToDecrypt.Text, txtDecryptedFile.Text, decryptedKey);

                }
                else if (radAES1.IsChecked == true)
                {
                    String sKey = txtKeyToDecrypt.Text;
                    String decryptedKey = encrypt.RSA.Decrypt(txtdKeys.Text, sKey, txtReceiver.Text);
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

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (txtSender.Text != "" && txtReceiver.Text != "")
            {
                lbl1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                label.Visibility = Visibility.Visible;
                label_Copy.Visibility = Visibility.Visible;
                btnEncrypt.Visibility = Visibility.Visible;
                btnGenerate.Visibility = Visibility.Visible;
                btnHash.Visibility = Visibility.Visible;
                BtnEncryptedFilePath.Visibility = Visibility.Visible;
                BtnFileToEncryptPath.Visibility = Visibility.Visible;
                BtnHashPath.Visibility = Visibility.Visible;
                BtnKeyPath.Visibility = Visibility.Visible;
                txtKeyPath.Visibility = Visibility.Visible;
                txtFileToEncryptPath.Visibility = Visibility.Visible;
                txtHashPath.Visibility = Visibility.Visible;
                txtEncryptedKey.Visibility = Visibility.Visible;
                txtEncryptedFilePath.Visibility = Visibility.Visible;
                btnEncryptedKey.Visibility = Visibility.Visible;
                radAES.Visibility = Visibility.Visible;
                radDES.Visibility = Visibility.Visible;

                txtReceiver.Visibility = Visibility.Hidden;
                txtSender.Visibility = Visibility.Hidden;
                btnStart.Visibility = Visibility.Hidden;
                label.Visibility = Visibility.Hidden;
                label_Copy.Visibility = Visibility.Hidden;
            }
        }

        #region Stenografie
        private void browseImageButton_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Image Files (*.jpeg; *.png; *.bmp)| *.jpg; *.png; *.bmp";
            if (browseVenster.ShowDialog() == true)
            {
                string padFoto = browseVenster.FileName;
                //FileNameLabel.Content = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(padFoto);
                bitmap.EndInit();
                selectedImage.Source = bitmap;
                // labelSelectedImage.Content = padFoto;
                imagePath = padFoto;
            }
        }

        private void HideData(object sender, RoutedEventArgs e)
        {
            try
            {
                string _text = richTextBox.Text;

                // string padFoto = labelSelectedImage.Content.ToString();
                Bitmap bitmap = new Bitmap(imagePath);

                if (_text.Equals(""))
                {
                    MessageBox.Show("The text you want to hide can't be empty", "Warning");
                    return;
                }

                if (encrypedCheckBox.IsChecked == true)
                {
                    if (passwordTextBox.Text.Length < 6)
                    {
                        MessageBox.Show("Please enter a password with at least 6 characters", "Warning");
                        return;
                    }
                    else
                    {
                        _text = StenografieCrypto.EncryptStringAES(_text, passwordTextBox.Text);
                    }
                }

                bitmap = StenografieHelper.embedText(_text, bitmap);

                MessageBox.Show("Your text was hidden in the image successfully!", "Done");
                maakLeeg();

                SaveFileDialog save_dialog = new SaveFileDialog();
                save_dialog.Filter = "Png Image|*.png|Bitmap Image|*.bmp";

                if (save_dialog.ShowDialog() == true)
                {
                    switch (save_dialog.FilterIndex)
                    {
                        case 0:
                        {
                            bitmap.Save(save_dialog.FileName, ImageFormat.Png);
                        }
                            break;
                        case 1:
                        {
                            bitmap.Save(save_dialog.FileName, ImageFormat.Bmp);
                        }
                            break;
                    }


                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong");
                // throw;
            }
            finally
            {
                maakLeeg();
            }
        }

        /// <summary>
        /// Maar de invoervelden leeg
        /// </summary>
        private void maakLeeg()
        {
            richTextBox.Text = "";
            passwordTextBox.Text = "";
            encrypedCheckBox.IsChecked = false;
        }
        #endregion

        private void discoverButton_Click(object sender, RoutedEventArgs e)
        {
            string padFoto = browseVenster.FileName;
            Bitmap bitmap = new Bitmap(padFoto);

            string extractedText = StenografieHelper.extractText(bitmap);

            if (encrypedCheckBox.IsChecked == true)
            {
                try
                {
                    extractedText = StenografieCrypto.DecryptStringAES(extractedText, passwordTextBox.Text);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Wrong password", "Error");

                    return;
                }
            }
            richTextBox.Text = extractedText;
        }
    }
    
}
