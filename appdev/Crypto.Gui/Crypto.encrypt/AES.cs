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
        public static string Encrypt
       (
           string bericht,  // message to be encrypted
           string passcode, // any string
           string saltValue, // any string
           string hashAlgorithm, // MD5 or SHA1
           int passwordIterations, // even number only          
           string initVector, // 16 bit string
           int grootte // 128 192 256
       )
        {
            // Convert strings to byte arrays
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] berichtBytes = Encoding.UTF8.GetBytes(bericht);

            // Use the passcode and salt values to derive a new password
            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                passcode,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations
            );

            // Use the password to generate a key, Specify the size of the key.
            byte[] sleutelBytes = password.GetBytes(grootte / 8);

            // Create Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // Set encryption mode to Chiper Block Chaining.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor
            (
                sleutelBytes,
                initVectorBytes
            );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                encryptor,
                CryptoStreamMode.Write
            );

            // encrypt
            cryptoStream.Write(berichtBytes, 0, berichtBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted message to a string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }


        public static string Decrypt
        (
            string cipherText,
            string passcode,
            string saltValue,
            string hashAlgorithm,
            int passwordIterations,
            string initVector,
            int grootte
        )
        {
            // Convert Strings into byte arrays.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // Use the passcode and salt values to derive a new password
            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                passcode,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations
            );

            // Use the password to generate a key, Specify the size of the key.
            byte[] sleutelBytes = password.GetBytes(grootte / 8);

            // Create Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // Set encryption mode to Chiper Block Chaining.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor
            (
                sleutelBytes,
                initVectorBytes
            );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                decryptor,
                CryptoStreamMode.Read
            );

            byte[] berichtBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read
            (
                berichtBytes,
                0,
                berichtBytes.Length
            );

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            string bericht = Encoding.UTF8.GetString
            (
                berichtBytes,
                0,
                decryptedByteCount
            );

            // Return decrypted string.   
            return bericht;
        }

    }
}

