using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Crypto.encrypt
{
    public class DES
    {
        // key has to be 8 chars
        static byte[] sleutel = ASCIIEncoding.ASCII.GetBytes("Testkeys");

        public static void Encrypt(string path, string destination, string sKey)
            {

            byte[] byteKey = ASCIIEncoding.ASCII.GetBytes(sKey);
            // throws exception if message is empty or null
                // initialize streams
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateEncryptor(byteKey, byteKey), CryptoStreamMode.Write);

            

                // encrypt the message
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(File.ReadAllText(path));
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();
                

                // return encrypted message
                string encrypted = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            File.WriteAllText(destination + "\\EncryptedFile.txt", encrypted);
            }

        public static string GenerateKey()
        {
            DESCryptoServiceProvider desProv = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desProv.Key);
        }
 
         public static void Decrypt(string path, string destination, string sKey)
            {
            byte[] byteKey = ASCIIEncoding.ASCII.GetBytes(sKey);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(File.ReadAllText(path)));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateDecryptor(byteKey, byteKey), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                File.WriteAllText(destination + "\\DecryptedFile.txt", reader.ReadToEnd());
            }
        }
    }


