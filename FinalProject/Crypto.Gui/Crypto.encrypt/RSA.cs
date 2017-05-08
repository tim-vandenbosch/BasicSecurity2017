using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Forms;

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

        // Aanmaken van de publieke en private keys op basis van naam.
        public static void Create(String path, String sender, String receiver)
        {
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();


            // rsa keys genereren
            _private_A = rsa1.ToXmlString(true);
            _public_A = rsa1.ToXmlString(false);
            _private_B = rsa2.ToXmlString(true);
            _public_B = rsa2.ToXmlString(false);

            Directory.CreateDirectory(path + "\\Keys_" + sender);
            Directory.CreateDirectory(path + "\\Keys_" + receiver);


            // Wegschrijven rsa keys
            File.WriteAllText(path + "\\Keys_" + sender + "\\Public_" + sender + ".txt", _public_A);
            File.WriteAllText(path + "\\Keys_" + sender + "\\Private_" + sender + ".txt", _private_A);
            File.WriteAllText(path + "\\Keys_" + receiver + "\\Private_" + receiver + ".txt", _private_B);
            File.WriteAllText(path + "\\Keys_" + receiver + "\\Public_" + receiver + ".txt", _public_B);
        }


        // Encrypteren van de symetrische key met behulp van de publieke key van de ontvanger.  
        public static void Encrypt(string encryptionPath, string rsaKeys, string sKey, string receiver)
        {
            var rsa = new RSACryptoServiceProvider();


                string publicB = File.ReadAllText(rsaKeys + "\\Keys_" + receiver + "\\Public_" + receiver + ".txt"); // publieke key ontvanger inlezen.

                rsa.FromXmlString(publicB); //encrypteren met de publickey sender
                var dataToEncrypt = _encoder.GetBytes(sKey); // Key omzetten naar byte array;
                var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray(); // Encrypteren van de sKey
                var length = encryptedByteArray.Count();
                var item = 0;
                var sb = new StringBuilder();
                foreach (var x in encryptedByteArray)
                {
                    item++;
                    sb.Append(x);

                    if (item < length) // nummers scheiden met een ,
                        sb.Append(", ");
                }
                File.WriteAllText(encryptionPath + "\\EncryptedKey.txt", sb.ToString()); // Wegschrijven naar gekozen path met file name EncryptedKey.txt

 
            
            //var dataToEncrypt = _encoder.GetBytes(File.ReadAllText(data));
        }

        public static string Decrypt(string rsaKeys, string sKey, string receiver) // Decrypteren van de geëncrypteerde key
        {

            var rsa = new RSACryptoServiceProvider();
            var fdata = File.ReadAllText(sKey); // inlezen geëncrypteerde key
            var dataArray = fdata.Split(new char[] { ',' }); // ingelezen key splitsen op ,
            byte[] dataByte = new byte[dataArray.Length]; 
            for (int i = 0; i < dataArray.Length; i++) 
            {
                dataByte[i] = Convert.ToByte(dataArray[i]); // key omzetten naar een byte array
            }

            string priveB = File.ReadAllText(rsaKeys + "\\Keys_" + receiver + "\\Private_" + receiver + ".txt"); // private key van de ontvanger inlezen

            rsa.FromXmlString(priveB); //decryteren met de privatekey sender
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }
        
        public static void Handteken(string message, string path, string keyPath, string sender) // de file hashen en handtekenen
        {
            SHA1Managed sha1 = new SHA1Managed(); 

            UnicodeEncoding encoding = new UnicodeEncoding();

            var priv_key = File.ReadAllText(keyPath + "\\Keys_" + sender + "\\Private_" + sender + ".txt"); // inlezen private key van verstuurder
            rsa.FromXmlString(priv_key); 

            byte[] data = encoding.GetBytes(File.ReadAllText(message)); // Te encrypteren bestand omzetten naar een byte array
            byte[] hash = sha1.ComputeHash(data); // een SHA1 hash maken van de data
            byte[] signature = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA1")); // de hash signen

            File.WriteAllText(path + "\\hash.txt", Convert.ToBase64String(signature)); // gesignede hash wegschrijven
        }
        public static bool Verify(String original, String signed, String keyPath, string sender) // de handtekening controleren
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            SHA1Managed sha1 = new SHA1Managed();
            bool result = true;
            var pub_key = File.ReadAllText(keyPath + "\\Keys_" + sender + "\\Public_" + sender + ".txt"); // inlezen public key verstuurder
            byte[] data = encoding.GetBytes(File.ReadAllText(original)); // omzetten bericht naar byte array
            byte[] hash = sha1.ComputeHash(data); // hash maken van origineel bericht

            byte[] signature = encoding.GetBytes(File.ReadAllText(signed)); // handtekening omzetten naar byte array

            rsa.FromXmlString(pub_key);

            var nresult = rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature); // handtekening verifiëren
            return result;

        }

    }
}
