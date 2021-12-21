namespace Cashregister.Smartcard
{

    using System;
    using PCSC.Iso7816;
    using System.Security.Cryptography.X509Certificates;
    using System.Numerics;

    class SmardCard_ACOS : AbstractCashRegisterSmardCard
    {

        private static readonly byte[] AID_SIG = new byte[] { 0xA0, 0x00, 0x00, 0x01, 0x18, 0x45, 0x43 };
        private static readonly byte[] AID_DEC = new byte[] { 0xA0, 0x00, 0x00, 0x01, 0x18, 0x45, 0x4E };
        private static readonly byte[] DF_SIG = new byte[] { 0xDF, 0x70 };
        private static readonly byte[] DF_DEC = new byte[] { 0xDF, 0x71 };
        private static readonly byte[] EF_C_CH_DS = new byte[] { 0xc0, 0x02 };
        // private static readonly byte[] EF_CIN_CSN = new byte[] { 0xD0, 0x01 };
        private static readonly byte[] TLV = new byte[] { 0x84, 0x01, 0x88, 0x80, 0x01, 0x44 };

        public SmardCard_ACOS(IsoReader toSet)
            : base(toSet)
        {

            if (ApplicationsMissing())
            {
                throw new Exception("Wrong card");
            }
        }

        public override void PrepareSignature()
        {
            SendSelectFID(MASTER_FILE);
            SendSelectFID(DF_SIG);
            Response response2 = SendCase3APDU(isoReader, 0x00, 0x22, 0x41, 0xb6, TLV);
            ThrowExceptionIfErrornous(response2.StatusWord);
        }

        public override byte[] Sign(String pin, byte[] SHA256HASH)
        {
            byte[] formatedPIN = CashRegisterUtil.GetFormat1PIN(pin);
            PrepareSignature();
            Response response3 = SendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x81, formatedPIN);
            ThrowExceptionIfErrornous(response3.StatusWord);
            Response response4 = SendCase3APDU(isoReader, 0x00, 0x2A, 0x90, 0x81, SHA256HASH);
            ThrowExceptionIfErrornous(response4.StatusWord);
            Response response5 = SendCase2APDU(isoReader, 0x00, 0x2A, 0x9E, 0x9A);
            ThrowExceptionIfErrornous(response5.StatusWord);
            byte[] signature = response5.GetData();
            return signature;
        }

        public override byte[] SignWithoutSelection(String pin, byte[] SHA256HASH)
        {
            byte[] formatedPIN = CashRegisterUtil.GetFormat1PIN(pin);
            Response response3 = SendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x81, formatedPIN);
            ThrowExceptionIfErrornous(response3.StatusWord);
            Response response4 = SendCase3APDU(isoReader, 0x00, 0x2A, 0x90, 0x81, SHA256HASH);
            ThrowExceptionIfErrornous(response4.StatusWord);
            Response response5 = SendCase2APDU(isoReader, 0x00, 0x2A, 0x9E, 0x9A);
            ThrowExceptionIfErrornous(response5.StatusWord);
            byte[] signature = response5.GetData();
            return signature;
        }

        public override string ReadCertificateSerialDecimal()
        {
            BigInteger l = GetCertificateSerial(DF_SIG, EF_C_CH_DS);
            string dez = l.ToString();
            return dez;
        }

        public override string ReadCertificateSerialHex()
        {
            BigInteger l = GetCertificateSerial(DF_SIG, EF_C_CH_DS);
            string hex = l.ToString("X");
            return hex;
        }

        public override X509Certificate2 ReadCertificate()
        {
            return GetCertificate(DF_SIG, EF_C_CH_DS);
        }

        public override string ReadCIN()
        {
            SendSelectFID(MASTER_FILE);
            SendSelectFID(DF_DEC);
            // or by using the long form
            // CommandApdu apdu0 = generateDataAPDU(isoReader, 0x00, 0xA4, 0x00, 0x0C, EF_CIN_CSN);
            // Response response0 = isoReader.Transmit(apdu0);
            // CommandApdu apdu = generateResponseAPDU(isoReader, 0x00, 0xB0, 0x00, 0x00); // plain
            Response response = SendCase2APDU(isoReader, 0x00, 0xB0, 0x86, 0x00); // short id
            byte[] data = response.GetData();
            string cin = CashRegisterUtil.ByteArrayToHexString(data);
            return cin;
        }

        private Boolean ApplicationsMissing()
        {
            Boolean applicationsMissing = false;
            Response response1 = SendSelectAID(AID_DEC);
            if (response1.StatusWord != 0x9000)
            {
                applicationsMissing = true;
            }
            Response response2 = SendSelectAID(AID_SIG);
            if (response2.StatusWord != 0x9000)
            {
                applicationsMissing = true;
            }
            return applicationsMissing;
        }
    }
}
