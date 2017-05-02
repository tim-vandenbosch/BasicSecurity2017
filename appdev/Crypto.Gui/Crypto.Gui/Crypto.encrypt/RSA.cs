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

        public static void Create(String path)
        {
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();

            _private_A = rsa1.ToXmlString(true);
            _public_A = rsa1.ToXmlString(false);
            _private_B = rsa2.ToXmlString(true);
            _public_B = rsa2.ToXmlString(false);

            Directory.CreateDirectory(path + "\\Keys_A");
            Directory.CreateDirectory(path + "\\Keys_B");


            
            File.WriteAllText(path + "\\Keys_A\\Public_A.txt", _public_A);
            File.WriteAllText(path + "\\Keys_A\\Private_A.txt", _private_A);
            File.WriteAllText(path + "\\Keys_B\\Private_B.txt", _private_B);
            File.WriteAllText(path + "\\Keys_B\\Public_B.txt", _public_B);
        }



        public static void Encrypt(string encryptionPath,string rsaKeys, string data)
        {
            var rsa = new RSACryptoServiceProvider();

            string publicB = File.ReadAllText(rsaKeys + "\\Keys_B\\Public_B.txt");
 
            rsa.FromXmlString(publicB); //encrypteren met de publickey sender
            //var dataToEncrypt = _encoder.GetBytes(File.ReadAllText(data));
            var dataToEncrypt = _encoder.GetBytes(data);
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
            File.WriteAllText(encryptionPath + "\\EncryptedKey.txt", sb.ToString());
        }

        public static string Decrypt(string rsaKeys, string data)
        {

            var rsa = new RSACryptoServiceProvider();
            var fdata = File.ReadAllText(data);
            var dataArray = fdata.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            string priveB = File.ReadAllText(rsaKeys + "\\Keys_B\\Private_B.txt");

            rsa.FromXmlString(priveB); //decryteren met de privatekey sender
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }
        
        public static void Handteken(string message, string path)
        {
            byte[] signed;
            var rsa = new RSACryptoServiceProvider();
            UnicodeEncoding encoder = new UnicodeEncoding();
            byte[] original = encoder.GetBytes(File.ReadAllText(message));

            SHA1Managed sha1 = new SHA1Managed();
            byte[] hash = sha1.ComputeHash(original);

            signed = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            string signedhash = Convert.ToBase64String(signed);

            File.WriteAllText(path + "\\hash.txt", signedhash);            

        }
        public static bool Verify(String original, String signed)
        {
            UnicodeEncoding encoder = new UnicodeEncoding();
            var rsa = new RSACryptoServiceProvider();
            SHA1Managed sha1 = new SHA1Managed();

            byte[] data = encoder.GetBytes(File.ReadAllText(original));
            byte[] signedb = encoder.GetBytes(File.ReadAllText(signed));

            byte[] hash = sha1.ComputeHash(data);

            return rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signedb);
            
            
        }

    }
}
