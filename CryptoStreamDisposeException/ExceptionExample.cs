using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoStreamDisposeException
{
    class ExceptionExample
    {
        private AesManaged algorithm;
        private byte[] key;
        private byte[] iv;

        public ExceptionExample()
        {
            algorithm = new AesManaged();
            //algorithm.Padding = PaddingMode.None;
            var deriveBytes = new Rfc2898DeriveBytes("password", Encoding.Unicode.GetBytes("salt"));
            key = deriveBytes.GetBytes(algorithm.KeySize / 8);
            iv = deriveBytes.GetBytes(algorithm.BlockSize / 8);
        }

        public byte[] Encrypt(string testText)
        {
            using (var dataStream = new MemoryStream(Encoding.Unicode.GetBytes(testText)))
            using (var transform = algorithm.CreateEncryptor(key, iv))
            using (var cryptoStream = new CryptoStream(dataStream, transform, CryptoStreamMode.Read))
            using (var encryptedDataStream = new MemoryStream())
            {
                var buffer = new byte[5000];
                var readed = cryptoStream.Read(buffer, 0, buffer.Length);
                while (readed > 0)
                {
                    encryptedDataStream.Write(buffer, 0, readed);
                    readed = cryptoStream.Read(buffer, 0, buffer.Length);
                }

                return encryptedDataStream.ToArray();
            }
        }

        public string Decrypt(byte[] encryptedData, bool interruptDecryption = false)
        {
            using (var encryptedDataStream = new MemoryStream(encryptedData))
            using (var transform = algorithm.CreateDecryptor(key, iv))
            using (var cryptoStream = new CryptoStream(encryptedDataStream, transform, CryptoStreamMode.Read))
            using (var dataStream = new MemoryStream())
            {
                var buffer = new byte[5000];
                var readed = cryptoStream.Read(buffer, 0, buffer.Length);
                while (readed > 0)
                {
                    dataStream.Write(buffer, 0, readed);

                    if (interruptDecryption)
                    {
                        throw new TestException("Something have happend.");
                    }

                    readed = cryptoStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.Unicode.GetString(dataStream.ToArray());
            }
        }
    }
}
