using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Crypto.encrypt;

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
    }
}

