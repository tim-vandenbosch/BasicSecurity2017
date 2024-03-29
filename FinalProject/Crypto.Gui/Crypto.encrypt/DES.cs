﻿using System;
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
        public static void Encrypt(string path, string destination, string sKey) // Encrypteren van het bestand met behulp van de sKey
            {

                byte[] byteKey = ASCIIEncoding.ASCII.GetBytes(sKey); // Key omzetten naar byte array

                //Streams initialiseren
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateEncryptor(byteKey, byteKey), CryptoStreamMode.Write);

            

                // Encrypteren van het bericht
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(File.ReadAllText(path));
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();
                

                // Geëncrypteerd bericht omzetten naar string en wegschrijven.
                string encrypted = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                File.WriteAllText(destination + "\\EncryptedFile.txt", encrypted);
            }

        public static string GenerateKey() // DES Key aanmaken
        {
            DESCryptoServiceProvider desProv = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desProv.Key);
        }
 
         public static void Decrypt(string path, string destination, string sKey) // Decrypteren van het geëncrypteerd bericht
            {
                byte[] byteKey = ASCIIEncoding.ASCII.GetBytes(sKey);

                // Initialiseren van de streams
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(File.ReadAllText(path)));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateDecryptor(byteKey, byteKey), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                File.WriteAllText(destination + "\\DecryptedFile.txt", reader.ReadToEnd()); // Geënecrypteerd bericht wegschrijven
            }
        }
    }


