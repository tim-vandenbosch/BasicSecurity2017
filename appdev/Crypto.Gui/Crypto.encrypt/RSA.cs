using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Microsoft.Win32;

namespace Crypto.encrypt
{
    public class RSA
    {
        private static string _private_A;
        private static string _public_A;
        private static string _private_B;
        private static string _public_B;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public static void Create()
        {
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();

            _private_A = rsa1.ToXmlString(true);
            _public_A = rsa1.ToXmlString(false);
            _private_B = rsa2.ToXmlString(true);
            _public_B = rsa2.ToXmlString(false);

            File.WriteAllText(@"C:\TestCrypto\Keys_A\Private_A.txt", _private_A);
            File.WriteAllText(@"C:\TestCrypto\Keys_A\Public_A.txt", _public_A);
            File.WriteAllText(@"C:\TestCrypto\Keys_B\Private_B.txt", _private_B);
            File.WriteAllText(@"C:\TestCrypto\Keys_B\Public_B.txt", _public_B);
        }



        public static string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();

            string publicB = File.ReadAllText(@"C:\TestCrypto\Keys_B\Public_B.txt");
 
            rsa.FromXmlString(publicB); //encrypteren met de publickey sender
            var dataToEncrypt = _encoder.GetBytes(File.ReadAllText(data));
            //var dataToEncrypt = _encoder.GetBytes(data));
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(", ");
            }
            File.WriteAllText(@"C:\TestCrypto\Keys_A\Encrypted.txt", sb.ToString());
            return sb.ToString();
        }

        public static string Decrypt(string data)
        {
            string datat = File.ReadAllText(data);

            var rsa = new RSACryptoServiceProvider();
            var dataArray = datat.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            string priveB = File.ReadAllText(@"C:\TestCrypto\Keys_B\Private_B.txt");

            rsa.FromXmlString(priveB); //decryteren met de privatekey sender
            var decryptedByte = rsa.Decrypt(dataByte, false);
            File.WriteAllText(@"C:\TestCrypto\Keys_A\Decrypted.txt", _encoder.GetString(decryptedByte));
            return File.ReadAllText("C:\\TestCrypto\\Keys_A\\Decrypted.txt");
        }


    }
}
