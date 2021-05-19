using System;
using PCSC;
using PCSC.Iso7816;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Cashregister.Smartcard;

namespace SampleClient {

    class Program {

        private static void Main() {

            using (var context = new SCardContext()) {
                String readerName = getReaderName(context);
                if (readerName == null) {
                    return;
                }
                using (IsoReader isoReader = new IsoReader(context, readerName, SCardShareMode.Shared, SCardProtocol.Any, false)) {
                    testSmartcard(isoReader);
                }
                context.Release();
            }
            Console.ReadKey();
        }

        private static void testSmartcard(IsoReader isoReader) {
            ICashRegisterSmartCard smartCard = CashRegisterSmartCardFactory.createInstance(isoReader);
            string cin = smartCard.readCIN();
            Console.WriteLine("CIN:  " + cin);
            String certificateSerialDecimal = smartCard.readCertificateSerialDecimal();
            String certificateSerialHex = smartCard.readCertificateSerialHex();
            Console.WriteLine("Certificate serial in dec " + certificateSerialDecimal + " (0x" + certificateSerialHex + ")");

            X509Certificate2 cert = smartCard.readCertificate();
            Console.WriteLine("Reading the certificate successful");

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
            for (int i = 0; i < 10; i++) {
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                signature = smartCard.sign(pin, hash);
                sw.Stop();
                Console.WriteLine("time taken for signature with selection:  " + sw.ElapsedMilliseconds + " ms");
            }

            smartCard.prepareSignature();
            for (int i = 0; i < 10; i++) {
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                signature = smartCard.signWithoutSelection(pin, hash);
                sw.Stop();
                Console.WriteLine("time taken for signature without selection:  " + sw.ElapsedMilliseconds + " ms");
            }
            string s3 = Convert.ToBase64String(signature);
            Console.WriteLine("Signature:  " + s3);
            bool bOK = smartCard.verify(signature, text);
            Console.WriteLine("Verified: " + bOK);
        }

        private static String getReaderName(SCardContext context) {
            context.Establish(SCardScope.System);
            var readerNames = context.GetReaders();
            if (readerNames == null || readerNames.Length < 1) {
                Console.WriteLine("You need at least one reader in order to run this example.");
                Console.ReadKey();
                return null;
            }
            String readerName = ChooseReader(readerNames);
            return readerName;
        }

        private static string ChooseReader(string[] readerNames) {
            Console.WriteLine("Available readers: ");
            for (var i = 0; i < readerNames.Length; i++) {
                Console.WriteLine("[" + i + "] " + readerNames[i]);
            }
            Console.Write("Which reader should be used? ");
            var line = Console.ReadLine();
            int choice;
            if (int.TryParse(line, out choice) && (choice >= 0) && (choice <= readerNames.Length)) {
                return readerNames[choice];
            }
            Console.WriteLine("An invalid number has been entered.");
            Console.ReadKey();
            return null;
        }

    }
}
