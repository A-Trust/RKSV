using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace Registrierkasse
{
    class Program
    {
        static void Test1()
        {
            Console.WriteLine("TEST1 - A-SIT Test Daten Entschlüsseln");
 
            string kassenID = "DEMO-CASH-BOX426";
            Console.WriteLine("Kassen ID: " + kassenID);
 
            string belegnummer = "776732";
            Console.WriteLine("Belegnummer: " + belegnummer);
 
            string aesB64 = "K7L9NyWiC1xKuyBO/7DuoRFqsqgq+m3t8hjOCMNXBio=";
            byte[] aesBytes = Convert.FromBase64String(aesB64);
            string encB64 = "4VjrD6b2vGo=";
            byte[] encBytes = Convert.FromBase64String(encB64);
 
            long umsatz = RKAesIcm.Decrypt(encBytes, kassenID, belegnummer, aesBytes);
            Console.WriteLine("Entschlüsselter Umsatz : " + umsatz);
            Console.WriteLine("");
        }
 
        static void Test2()
        {
            Console.WriteLine("TEST2 - Verschlüsseln/Entschlüsseln");
 
            // Generate new AES Key
            byte[] aesKey = RKAesIcm.GenerateKey();
            Console.WriteLine("AES key (B64): " + Convert.ToBase64String(aesKey));
 
            string kassenID = "Register3874";
            Console.WriteLine("Kassen ID: " + kassenID);
 
            string belegnummer = "39920034";
            Console.WriteLine("Belegnummer: " + belegnummer);
 
            long umsatz = 2349; // Euro-Cent
            Console.WriteLine("Umsatz: " + umsatz);
 
            byte[] encryptedBytes = RKAesIcm.Encrypt(umsatz, kassenID, belegnummer, aesKey);
            Console.WriteLine("Verschlüsselter Umsatz (B64): " + Convert.ToBase64String(encryptedBytes));
 
            long umsatz2 = RKAesIcm.Decrypt(encryptedBytes, kassenID, belegnummer, aesKey);
            Console.WriteLine("Entschlüsselter Umsatz : " + umsatz2);
            Console.WriteLine("");
 
        }
 
        static void Main(string[] args)
        {
            // TEST 1 - A-SIT Test Daten Entschlüsseln
            Test1();
 
            // TEST2 - Verschlüsseln/Entschlüsseln
            Test2();
 
            Console.ReadLine();
        }
    }
}