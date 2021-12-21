namespace Cashregister.Smartcard
{

    using PCSC.Iso7816;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Numerics;

    abstract class AbstractCashRegisterSmardCard : ICashRegisterSmartCard
    {

        protected static readonly byte[] MASTER_FILE = new byte[] { 0x3F, 0x00 };

        protected readonly IsoReader isoReader;

        public abstract X509Certificate2 ReadCertificate();

        public abstract byte[] Sign(String pin, byte[] SHA256HASH);

        public abstract byte[] SignWithoutSelection(String pin, byte[] SHA256HASH);

        public abstract string ReadCertificateSerialDecimal();

        public abstract string ReadCertificateSerialHex();

        public abstract string ReadCIN();

        public abstract void PrepareSignature();

        protected AbstractCashRegisterSmardCard(IsoReader toSet)
        {
            isoReader = toSet;
        }

        protected X509Certificate2 GetCertificate(byte[] DF, byte[] EF)
        {
            byte[] certBytes = GetBuffer(false, DF, EF);
            X509Certificate2 cert = new X509Certificate2(certBytes);
            return cert;
        }

        protected BigInteger GetCertificateSerial(byte[] DF, byte[] EF)
        {
            byte[] ba = GetBuffer(true, DF, EF);
            byte length = ba[14];
            BigInteger res = 0;
            for (int i = 0; i < length; i++)
            {
                res = (res << 8) + ba[15 + i];
            }
            return res;
        }

        protected byte[] GetBuffer(bool onlyFirst, byte[] DF, byte[] EF)
        {
            SendSelectFID(MASTER_FILE);
            SendSelectFID(DF);
            SendSelectFID(EF);
            byte[] result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                int offset = 0;
                while (true)
                {
                    Response response = SendCase2APDU(isoReader, 0x00, 0xB0, (byte)(offset >> 8), (byte)offset);
                    if (response.StatusWord != 0x9000)
                    {
                        break;
                    }
                    byte[] responseData = response.GetData();
                    ms.Write(responseData, 0, responseData.Length);
                    if (onlyFirst)
                    {
                        break;
                    }
                    offset += 256;
                }
                result = ms.ToArray();
            }
            return result;
        }

        protected static Response SendCase2APDU(IsoReader isoReader, byte cla, byte ins, byte p1, byte p2, int le = 0)
        {
            CommandApdu c = new CommandApdu(IsoCase.Case2Short, isoReader.ActiveProtocol)
            {
                CLA = cla,
                INS = ins,
                P1 = p1,
                P2 = p2,
                Le = le
            };
            Response toReturn = isoReader.Transmit(c);
            return toReturn;
        }

        protected static Response SendCase3APDU(IsoReader isoReader, byte cla, byte ins, byte p1, byte p2, byte[] data)
        {
            CommandApdu c = new CommandApdu(IsoCase.Case3Short, isoReader.ActiveProtocol)
            {
                CLA = cla,
                INS = ins,
                P1 = p1,
                P2 = p2,
                Data = data
            };
            Response toReturn = isoReader.Transmit(c);
            return toReturn;
        }

        protected Response SendCase4APDU(byte cla, byte ins, byte p1, byte p2, byte[] data, byte le)
        {
            CommandApdu c = new CommandApdu(IsoCase.Case4Short, isoReader.ActiveProtocol)
            {
                CLA = cla,
                INS = ins,
                P1 = p1,
                P2 = p2,
                Data = data,
                Le = le
            };
            Response toReturn = isoReader.Transmit(c);
            return toReturn;
        }

        protected Response ReadRecord(byte recordId)
        {
            Response response = SendCase2APDU(isoReader, 0x00, 0xB2, recordId, 0x04);
            return response;
        }

        protected Response SendSelectFID(byte[] fileId)
        {
            Response response = SendCase3APDU(isoReader, 0x00, 0xA4, 0x00, 0x0C, fileId);
            ThrowExceptionIfErrornous(response.StatusWord);
            return response;
        }

        protected Response SendSelectAID(byte[] applicationId)
        {
            Response response = SendCase3APDU(isoReader, 0x00, 0xA4, 0x04, 0x0C, applicationId);
            return response;
        }

        public bool Verify(byte[] signature, byte[] signedData)
        {
            byte[] publicKeyEncoded = CashRegisterUtil.GetEncodedKey(ReadCertificate());
            CngKey cngKey = CngKey.Import(publicKeyEncoded, CngKeyBlobFormat.EccPublicBlob);
            ECDsaCng ecdsaCng = new ECDsaCng(cngKey);
            bool valid = ecdsaCng.VerifyData(signedData, signature);
            ecdsaCng.Dispose();
            return valid;
        }

        public void ThrowExceptionIfErrornous(int statusCode)
        {
            if (statusCode != 0x9000)
            {
                throw new Exception("Command failed with status " + statusCode);
            }
        }
    }
}
