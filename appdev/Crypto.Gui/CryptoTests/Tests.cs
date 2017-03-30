using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Crypto.encrypt;
using System.IO;

namespace CryptoTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ShouldDecryptMessageDES()
        {
            String message = "HelloWorld";
            String encrypted = DES.Encrypt(message);
            Assert.That(DES.Decrypt(encrypted).Equals("HelloWorld"));
        }
        [Test]
        public void ShouldDecryptMessageAES()
        {
            String message = "HelloWorld";
            String encrypted = AES.Encrypt(message, "Passwoordzin", "salt", "SHA1", 2, "abcdefghijlmnopq", 256);
            Assert.That(AES.Decrypt(encrypted, "Passwoordzin", "salt", "SHA1", 2, "abcdefghijlmnopq", 256).Equals(message));
        }

        [Test]
        public void ShouldCreateKeyFiles()
        {
            RSA.Create();
            Assert.That(File.Exists("C:\\TestCrypto\\Keys_A\\Private_A.txt"));
        }

        [Test]
        public void ShouldDecryptMessage()
        {
            RSA.Create();

            String encryptedMessage = RSA.Encrypt("C:\\TestCrypto\\Keys_A\\ToEncrypt.txt");
            String decryptedMessage = RSA.Decrypt("C:\\TestCrypto\\Keys_A\\Encrypted.txt");
            String result = decryptedMessage.Trim();
            Assert.That(result.Equals("HelloWorld!"));


        }
    }
}