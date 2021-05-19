using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Registrierkasse
{
    public class RKAesIcm
    {
        public static byte[] GenerateKey()
        {
            AesManaged aesKey = new AesManaged();
            aesKey.KeySize = 256;
            aesKey.GenerateKey();

            return aesKey.Key;
        }


        public static byte[] Encrypt(long umsatz, string kassenID, string belegnummer, byte[] aesKey)
        {
            byte[] data = EncodeUmsatz(umsatz);
            byte[] iv = GenerateIV(kassenID, belegnummer);

            return Encrypt(data, iv, aesKey);
        }

        public static long Decrypt(byte[] data, string kassenID, string belegnummer, byte[] aesKey)
        {
            long umsatz = -1;
            byte[] iv = GenerateIV(kassenID, belegnummer);
            byte[] decrypted = Decrypt(data, iv, aesKey);
            if (decrypted != null)
            {
                umsatz = DecodeUmsatz(decrypted);
            }

            return umsatz;
        }


        private static byte[] Encrypt(byte[] data, byte[] IV, byte[] aesKey)
        {
            byte[] encrypted = null;
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");

            try
            {
                KeyParameter skey = new KeyParameter(aesKey);
                ParametersWithIV aesIVKeyParam = new ParametersWithIV(skey, IV);
                cipher.Init(true, aesIVKeyParam);
            }
            catch (Exception e)
            {
                Console.WriteLine("Encrypt exception (init): " + e.Message);
                return null;
            }

            try
            {
                MemoryStream bOut = new MemoryStream();
                CipherStream cOut = new CipherStream(bOut, null, cipher);

                cOut.Write(data, 0, data.Length);
                cOut.Close();
                encrypted = bOut.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine("Encrypt exception: " + e.Message);
                encrypted = null;
            }

            return encrypted;


        }


        private static byte[] Decrypt(byte[] data, byte[] iv, byte[] aesKey)
        {
            byte[] output = null;

            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");

            try
            {
                KeyParameter skey = new KeyParameter(aesKey);
                ParametersWithIV aesIVKeyParam = new ParametersWithIV(skey, iv);
                cipher.Init(false, aesIVKeyParam);
            }
            catch (Exception e)
            {
                Console.WriteLine("Decrypt exception (init): " + e.Message);
                return output;
            }

            try
            {
                MemoryStream bIn = new MemoryStream(data, false);
                CipherStream cIn = new CipherStream(bIn, cipher, null);
                BinaryReader dIn = new BinaryReader(cIn);

                output = new byte[data.Length];
                Array.Clear(output, 0, output.Length);
                output = dIn.ReadBytes(output.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Decrypt exception : " + e.Message);
                return output;
            }

            return output;
        }


        private static byte[] GenerateIV(string kassenId, string belegNummer)
        {
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(kassenId + belegNummer);
            SHA256 hasher = SHA256Managed.Create();
            byte[] hashBytes = hasher.ComputeHash(inBytes);

            byte[] iv = new List<byte>(hashBytes).GetRange(0, 16).ToArray();

            return iv;
        }

        private static long DecodeUmsatz(byte[] umsatzBytes)
        {
            // reverse to get little-endian array
            Array.Reverse(umsatzBytes, 0, umsatzBytes.Length);
            return BitConverter.ToInt64(umsatzBytes, 0);
        }

        private static byte[] EncodeUmsatz(long umsatz)
        {
            // This gives an 8-byte array
            byte[] umsatzBytes = BitConverter.GetBytes(umsatz);
            // Pad with zeroes to get 16 bytes
            int length = 16 * ((umsatzBytes.Length + 15) / 16);
            Array.Resize(ref umsatzBytes, length);
            // reverse to get big-endian array
            Array.Reverse(umsatzBytes, 0, umsatzBytes.Length);
            return umsatzBytes;
        }
    }
}
