using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Crypto.encrypt
{
    public class AES
    {
        //Genereren van een AES sleutel
        public static string GenerateKey()
        {
            AesCryptoServiceProvider aes = (AesCryptoServiceProvider)AesCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(aes.Key);
        }

        //Encrypteren van een bestand met een gegeven sleutel.
        public static void Encrypt(string inputFile, string outputFile, string key)
        {
            try
            {
                //Genereren van een encrypteer key.
                byte[] keyBytes;
                keyBytes = Encoding.Unicode.GetBytes(key);

                Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);

                RijndaelManaged rijndaelCSP = new RijndaelManaged();
                rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
                rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

                ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();

                //Inlezen van het te geëncrypteren bestand
                FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

                byte[] inputFileData = new byte[(int)inputFileStream.Length];
                inputFileStream.Read(inputFileData, 0, (int)inputFileStream.Length);

                //Opstellen van een stream om het bestand weg te schrijven
                FileStream outputFileStream = new FileStream(outputFile + "\\EncryptedFile.txt", FileMode.Create, FileAccess.Write);

                //Bestand geëncrypteerd met de aangemaakte encryptor 
                CryptoStream encryptStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write);
                encryptStream.Write(inputFileData, 0, (int)inputFileStream.Length);
                encryptStream.FlushFinalBlock();

                rijndaelCSP.Clear();
                encryptStream.Close();
                inputFileStream.Close();
                outputFileStream.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Encryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //MessageBox.Show("File Encryption Complete!");

        }
        //Decrypteren van een bestand met AES via een gegeven sleutel.
        public static void Decrypt(string inputFile, string outputFile, string key)
        {
            try
            {
                //Genereren van een decrypteer key
                byte[] keyBytes = Encoding.Unicode.GetBytes(key);

                Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);

                RijndaelManaged rijndaelCSP = new RijndaelManaged();
                rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
                rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);
                ICryptoTransform decryptor = rijndaelCSP.CreateDecryptor();

                //Opstellen van een stream om het geëncrypteerde bestand te openen
                FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

                //Opstellen van een stream om het gedecrypteerde bestand te openen el al decrypteren met een 
                CryptoStream decryptStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read);

                byte[] inputFileData = new byte[(int)inputFileStream.Length];
                decryptStream.Read(inputFileData, 0, (int)inputFileStream.Length);

                FileStream outputFileStream = new FileStream(outputFile + "\\DecryptedFile.txt", FileMode.Create, FileAccess.Write);
                outputFileStream.Write(inputFileData, 0, inputFileData.Length);
                outputFileStream.Flush();
                
                rijndaelCSP.Clear();

                //Sluiten van de streams.
                decryptStream.Close();
                inputFileStream.Close();
                outputFileStream.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Decryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //   MessageBox.Show("File Decryption Complete!");
        }

    }
}


