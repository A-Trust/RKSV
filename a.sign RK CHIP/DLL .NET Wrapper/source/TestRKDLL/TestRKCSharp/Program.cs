using Registrierkasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TestRKCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            RKWrapper rkw = new RKWrapper();

            int ret = 0;
            string zdaId;
            string sigCertSerial;
            X509Certificate sigCert;
            X509Certificate issuerCert;

            ret = rkw.GetInfo(out zdaId,out sigCert,out issuerCert);

            if (ret== 0)
            {
                sigCertSerial = sigCert.GetSerialNumberString();
                Console.WriteLine("ZDA ID = " + zdaId);
                Console.WriteLine("Sig Cert Serial = " + sigCertSerial);

                byte[] sigCertBytes = sigCert.Export(X509ContentType.Cert);
                Console.WriteLine("Sig Cert B64:    " + Convert.ToBase64String(sigCertBytes));

                byte[] issCertBytes = issuerCert.Export(X509ContentType.Cert);
                Console.WriteLine("Issuer Cert B64: " + Convert.ToBase64String(issCertBytes));
            }
            else
            {
                Console.WriteLine("GetInfo failed with return code = 0x" + ret.ToString("X4"));
                Console.WriteLine("Last error : " + rkw.LastError);
            }

            string JWS_Protected_Header = Base64url("{\"alg\":\"ES256\"}");
            string JWS_Payload = Base64url("_R1-AT1_DEMO-CASH-BOX426_776730_2015-10-14T18:20:23_0,00_0,00_0,00_0,00_0,00_PT7P6PGD2KOQ2===_968935007593160625_PT7P6PGD2KOQ2===");

            string toBeSignedStr = string.Format("{0}.{1}", JWS_Protected_Header, JWS_Payload);
            byte[] tbsBytes = System.Text.Encoding.UTF8.GetBytes(toBeSignedStr);
            byte[] signature;

            ret = rkw.Sign(tbsBytes,out signature);

            if (ret == 0)
            {
                string sigB64 = Base64url(signature);
                Console.WriteLine("Signature:    " + sigB64);
            }
            else
            {
                Console.WriteLine("Sign failed with return code = 0x" + ret.ToString("X4"));
                Console.WriteLine("Last error : " + rkw.LastError);
            }

            Registrierkasse.RKWrapper.UnloadCryptoLibrary();

            Console.ReadLine();
        }

        public static string Base64url(string input)
        {
            return Base64url(System.Text.Encoding.UTF8.GetBytes(input));
        }

        public static string Base64url(byte[] inputBytes)
        {
            StringBuilder result = new StringBuilder(Convert.ToBase64String(inputBytes).TrimEnd('='));
            result.Replace('+', '-');
            result.Replace('/', '_');
            return result.ToString();
        }

    }

}
