using CryptoStreamDisposeException.Properties;
using System;

namespace CryptoStreamDisposeException
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 30);

            var example = new ExceptionExample();

            var encryptedData = example.Encrypt(Resources.TestText);
            Console.WriteLine("The test text has been sucessfully encrypted.");

            var decryptedText = example.Decrypt(encryptedData);
            if (decryptedText == Resources.TestText)
            {
                Console.WriteLine("The test text has been sucessfully decrypted.");

                Console.WriteLine();
                Console.WriteLine("Test disposing.");
                try
                {
                    decryptedText = example.Decrypt(encryptedData, true);
                }
                catch (TestException ex)
                {
                    Console.WriteLine("Decription has been interrupted, diposing has been sucessfully perfomed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Expected behavior: diposing has been sucessfully perfomed.");
                    Console.WriteLine("Actual behavior: unknow exception has been thrown during disposing.");
                    Console.WriteLine();
                    Console.WriteLine(ex);
                }
            }

            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
