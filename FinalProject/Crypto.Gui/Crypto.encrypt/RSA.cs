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
        private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public static void Create(String path, String sender, String receiver)
        {
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();

            _private_A = rsa1.ToXmlString(true);
            _public_A = rsa1.ToXmlString(false);
            _private_B = rsa2.ToXmlString(true);
            _public_B = rsa2.ToXmlString(false);

            Directory.CreateDirectory(path + "\\Keys_" + sender);
            Directory.CreateDirectory(path + "\\Keys_" + receiver);


            
            File.WriteAllText(path + "\\Keys_" + sender + "\\Public_" + sender + ".txt", _public_A);
            File.WriteAllText(path + "\\Keys_" + sender + "\\Private_" + sender + ".txt", _private_A);
            File.WriteAllText(path + "\\Keys_" + receiver + "\\Private_" + receiver + ".txt", _private_B);
            File.WriteAllText(path + "\\Keys_" + receiver + "\\Public_" + receiver + ".txt", _public_B);
        }



        public static void Encrypt(string encryptionPath,string rsaKeys, string data, string receiver)
        {
            var rsa = new RSACryptoServiceProvider();

            string publicB = File.ReadAllText(rsaKeys + "\\Keys_" + receiver + "\\Public_" + receiver + ".txt");
 
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

        public static string Decrypt(string rsaKeys, string data, string receiver)
        {

            var rsa = new RSACryptoServiceProvider();
            var fdata = File.ReadAllText(data);
            var dataArray = fdata.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            string priveB = File.ReadAllText(rsaKeys + "\\Keys_" + receiver + "\\Private_" + receiver + ".txt");

            rsa.FromXmlString(priveB); //decryteren met de privatekey sender
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }
        
        public static void Handteken(string message, string path, string keyPath, string sender)
        {
            SHA1Managed sha1 = new SHA1Managed();

            UnicodeEncoding encoding = new UnicodeEncoding();

            var priv_key = File.ReadAllText(keyPath + "\\Keys_" + sender + "\\Private_" + sender + ".txt");

            
            rsa.FromXmlString(priv_key);

            byte[] data = encoding.GetBytes(File.ReadAllText(message));
            byte[] hash = sha1.ComputeHash(data);
            byte[] signature = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));

            File.WriteAllText(path + "\\hash.txt", Convert.ToBase64String(signature));


            //byte[] signed;
            //var rsa = new RSACryptoServiceProvider();
            //UnicodeEncoding encoder = new UnicodeEncoding();
            //byte[] original = encoder.GetBytes(File.ReadAllText(message));

            //var priv_key = File.ReadAllText(keyPath + "\\Keys_A\\Private_A.txt");
            //SHA1Managed sha1 = new SHA1Managed();
            //byte[] hash = sha1.ComputeHash(original);

            //rsa.FromXmlString(priv_key);
            //signed = rsa.SignData(hash, "SHA1");




        }
        public static bool Verify(String original, String signed, String keyPath, string sender)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            SHA1Managed sha1 = new SHA1Managed();
            var pub_key = File.ReadAllText(keyPath + "\\Keys_" + sender + "\\Public_" + sender + ".txt");
            byte[] data = encoding.GetBytes(File.ReadAllText(original));
            byte[] hash = sha1.ComputeHash(data);

            byte[] signature = encoding.GetBytes(File.ReadAllText(signed));

            rsa.FromXmlString(pub_key);

            return rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);


            //byte[] data = encoder.GetBytes(File.ReadAllText(original));
            //byte[] signedb = (File.ReadAllBytes(signed));
            //rsa.FromXmlString(pub_key);
            //byte[] hash = sha1.ComputeHash(data);
            //return rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signedb);


        }

    }
}
