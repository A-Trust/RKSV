namespace Cashregister.Smartcard {

    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public sealed class CashRegisterUtil {

        private CashRegisterUtil() {
            // no instances
        }

        public static string byteArrayToHexString(byte[] data) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++) {
                builder.Append(data[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public static bool verify(byte[] signature, X509Certificate cert, byte[] signedData) {
            byte[] publicKeyEncoded = getEncodedKey(cert);
            CngKey cngKey = CngKey.Import(publicKeyEncoded, CngKeyBlobFormat.EccPublicBlob);
            ECDsaCng ecdsaCng = new ECDsaCng(cngKey);
            bool valid = ecdsaCng.VerifyData(signedData, signature);
            ecdsaCng.Dispose();
            return valid;
        }

        public static byte[] getEncodedKey(X509Certificate cert) {
            byte[] publicKey = cert.GetPublicKey();
            byte[] publicKeyEncoded = new byte[72];
            // Remove the first char (0x04) from the pub key and replace with the ECS1 header
            publicKeyEncoded[0] = 69; // E
            publicKeyEncoded[1] = 67; // C
            publicKeyEncoded[2] = 83; // S
            publicKeyEncoded[3] = 49; // 1
            publicKeyEncoded[4] = 32; // Key length
            publicKeyEncoded[5] = 0;
            publicKeyEncoded[6] = 0;
            publicKeyEncoded[7] = 0;
            for (int i = 0; i < 64; i++) {
                publicKeyEncoded[i + 8] = publicKey[i + 1];
            }
            return publicKeyEncoded;
        }

        public static byte[] getFormat1PIN(String pin) {
            //  { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x00, 0x00 };
            if (pin.Length != 6 && pin.Length != 4) {
                throw new Exception("Wrong PIN length");
            }
            char[] ca = pin.ToCharArray();
            byte[] ba = new byte[8];
            for (int i = 0; i < 8; i++) {
                if (i < ca.Length) {
                    ba[i] = (byte)ca[i];
                } else {
                    ba[i] = 0x00;
                }
            }
            return ba;
        }

        public static byte[] getFormat2PIN(String pin) {
            /*
            Format 2 PIN block
            The format 2 PIN block is constructed thus:
            1 nibble with the value of 2, which identifies this as a format 2 block
            1 nibble encoding the length N of the PIN
            N nibbles, each encoding one PIN digit
            14-N nibbles, each holding the "fill" value 15
            */
            if (pin.Length != 6 && pin.Length != 4) {
                throw new Exception("Wrong PIN length");
            }
            byte[] ba = new byte[8];
            ba[0] = (byte)((2 << 4) | pin.Length);
            char[] ca = pin.ToCharArray();
            byte b = (byte)(ca[0] - 0x30);
            ba[1] = (byte)(((ca[0] - 0x30) << 4) | (ca[1] - 0x30));
            ba[2] = (byte)(((ca[2] - 0x30) << 4) | (ca[3] - 0x30));
            if (pin.Length == 6) {
                ba[3] = (byte)(((ca[4] - 0x30) << 4) | (ca[5] - 0x30));
            } else {
                ba[3] = 0xFF;
            }
            ba[4] = 0xFF;
            ba[5] = 0xFF;
            ba[6] = 0xFF;
            ba[7] = 0xFF;
            return ba;
        }
    }
}
