using System;
using PCSC;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Cashregister.Smartcard;

namespace SampleClient
{

    class Program
    {

        private static void Main()
        {

            using (SCardContext context = new SCardContext())
            {
                String readerName = GetReaderName(context);
                if (readerName == null)
                {
                    return;
                }
                TestSmartcard(context, readerName);
                context.Release();
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void TestSmartcard(SCardContext context, string readerName)
        {
            ICashRegisterSmartCard smartCard = CashRegisterSmartCardFactory.CreateInstance(context, readerName);
            string cin = smartCard.ReadCIN();
            Console.WriteLine("CIN:  " + cin);
            String certificateSerialDecimal = smartCard.ReadCertificateSerialDecimal();
            String certificateSerialHex = smartCard.ReadCertificateSerialHex();
            Console.WriteLine("Certificate serial in dec " + certificateSerialDecimal + " (0x" + certificateSerialHex + ")");

            X509Certificate2 cert = smartCard.ReadCertificate();
            if(cert != null)
            {
                Console.WriteLine("Reading the certificate successful");
            }

            string textString = "Das ist ein Beispieltext der anstelle eines Registrierkasseneintrags signiert wird... ";
            byte[] text = Encoding.UTF8.GetBytes(textString);
            Console.WriteLine("Text: " + textString);
            SHA256Managed crypt = new SHA256Managed();
            byte[] hash = crypt.ComputeHash(text, 0, text.Length);
            crypt.Dispose();
            string hashString = Convert.ToBase64String(hash);
            Console.WriteLine("SHA256 Hash: " + hashString);
            String pin = "123456";

            byte[] signature = null;
            for (int i = 0; i < 10; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                signature = smartCard.Sign(pin, hash);
                sw.Stop();
                Console.WriteLine("time taken for signature with selection:  " + sw.ElapsedMilliseconds + " ms");
            }

            smartCard.PrepareSignature();
            for (int i = 0; i < 10; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                signature = smartCard.SignWithoutSelection(pin, hash);
                sw.Stop();
                Console.WriteLine("time taken for signature without selection:  " + sw.ElapsedMilliseconds + " ms");
            }
            string s3 = Convert.ToBase64String(signature);
            Console.WriteLine("Signature:  " + s3);
            bool bOK = smartCard.Verify(signature, text);
            Console.WriteLine("Verified: " + bOK);
        }

        private static String GetReaderName(SCardContext context)
        {
            context.Establish(SCardScope.System);
            var readerNames = context.GetReaders();
            if (readerNames == null || readerNames.Length < 1)
            {
                Console.WriteLine("You need at least one reader in order to run this example.");
                Console.ReadKey();
                return null;
            }
            String readerName = ChooseReader(readerNames);
            return readerName;
        }

        private static string ChooseReader(string[] readerNames)
        {
            Console.WriteLine("Available readers: ");
            for (var i = 0; i < readerNames.Length; i++)
            {
                Console.WriteLine("[" + i + "] " + readerNames[i]);
            }
            Console.Write("Which reader should be used? ");
            var line = Console.ReadLine();
            if (int.TryParse(line, out int choice) && (choice >= 0) && (choice <= readerNames.Length))
            {
                return readerNames[choice];
            }
            Console.WriteLine("An invalid number has been entered.");
            Console.ReadKey();
            return null;
        }

    }
}
