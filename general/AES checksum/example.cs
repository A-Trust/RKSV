using System;
using System.Security.Cryptography; 
 
namespace Registrierkasse
{
    class Program
    {
        static void Main(string[] args)
        {
            const string base64AESKey = "cWhay3H4asTvRzXzXGZQ3KyBEu9BZaIxl8J+L4Bhr5A=";
            const int N = 3;
            const string valSumCalc = "qx6p";
 
 
            SHA256Managed h = new SHA256Managed();
            byte[] base64AESKeyBytes = System.Text.Encoding.ASCII.GetBytes(base64AESKey);
            byte[] sha256hash = h.ComputeHash(base64AESKeyBytes);
 
            byte[] sha256hashNbytes = new byte[N];
            Array.Copy(sha256hash, 0, sha256hashNbytes, 0, sha256hashNbytes.Length);
 
            string base64sha256hashNbytes = Convert.ToBase64String(sha256hashNbytes);
 
            string result = base64sha256hashNbytes.Replace("=", "");
 
 
            if (0 == string.Compare(result, valSumCalc))
            {
                Console.WriteLine("success");
            }
            else
            {
                Console.WriteLine("error");
            }
 
            Console.ReadLine();
        }
    }
}