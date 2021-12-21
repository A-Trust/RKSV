namespace Cashregister.Smartcard
{

    using System.Numerics;
    using System;
    using PCSC.Iso7816;
    using System.Security.Cryptography.X509Certificates;

    class SmartCard_AcosID : AbstractCashRegisterSmardCard
    {

        private static readonly byte[] DF_SIG = new byte[] { 0xDF, 0x01 };

        private static readonly byte[] EF_CIN_CSN = new byte[] { 0xD0, 0x01 };
        private static readonly byte[] EF_C_CH_DS = new byte[] { 0xc0, 0x00 };

        private static readonly byte[] AID_SIG = new byte[] { 0xD0, 0x40, 0x00, 0x00, 0x22, 0x00, 0x01 };

        public SmartCard_AcosID(IsoReader toSet)
            : base(toSet)
        {
            if (ApplicationsMissing()) {
                throw new Exception("Wrong card");
            }
        }

        public override void PrepareSignature()
        {
            SendSelectFID(MASTER_FILE);
            SendSelectFID(DF_SIG);
        }

        public override byte[] Sign(String pin, byte[] SHA256HASH)
        {
            byte[] formatedPIN = CashRegisterUtil.GetFormat2PIN(pin);
            PrepareSignature();
            Response response3 = SendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x8A, formatedPIN);
            ThrowExceptionIfErrornous(response3.StatusWord);
            Response responseSign = SendCase4APDU(0x00, 0x2A, 0x9E, 0x9A, SHA256HASH, 64);
            ThrowExceptionIfErrornous(responseSign.StatusWord);
            byte[] signature = responseSign.GetData();
            return signature;
        }

        public override byte[] SignWithoutSelection(String pin, byte[] SHA256HASH)
        {
            byte[] formatedPIN = CashRegisterUtil.GetFormat2PIN(pin);
            Response response3 = SendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x8A, formatedPIN);
            ThrowExceptionIfErrornous(response3.StatusWord);
            Response responseSign = SendCase4APDU(0x00, 0x2A, 0x9E, 0x9A, SHA256HASH, 64);
            ThrowExceptionIfErrornous(responseSign.StatusWord);
            byte[] signature = responseSign.GetData();
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
            SendSelectFID(EF_CIN_CSN);
            Response response = SendCase2APDU(isoReader, 0x00, 0xB0, 0x00, 0x00); // plain
            byte[] data = response.GetData();
            string cin = CashRegisterUtil.ByteArrayToHexString(data);
            return cin;
        }

        private Boolean ApplicationsMissing()
        {
            Boolean applicationsMissing = false;
            Response response = SendSelectAID(AID_SIG);
            if (response.StatusWord != 0x9000)
            {
                applicationsMissing = true;
            }
            return applicationsMissing;
        }
    }
}
