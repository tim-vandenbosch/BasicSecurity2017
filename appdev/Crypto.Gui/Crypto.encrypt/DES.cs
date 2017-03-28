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

        public static string Encrypt(string bericht)
            {
                // throws exception if message is empty or null
                if (String.IsNullOrEmpty(bericht))
                {
                    throw new ArgumentNullException("Geef een geldige string in.");
                }
                // initialize streams
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateEncryptor(sleutel, sleutel), CryptoStreamMode.Write);

                // encrypt the message
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(bericht);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();
                
                // return encrypted message
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }


 
         public static string Decrypt(string bericht)
            {
                if (String.IsNullOrEmpty(bericht))
                {
                    throw new ArgumentNullException("Het bericht mag niet leeg of null zijn.");
                }

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(bericht));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateDecryptor(sleutel, sleutel), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
        }
    }


