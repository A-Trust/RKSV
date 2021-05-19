namespace Cashregister.Smartcard {

    using System;
    using PCSC.Iso7816;
    using System.Security.Cryptography.X509Certificates;
    using System.Numerics;

    class SmardCard_ACOS : AbstractCashRegisterSmardCard {

        private static readonly byte[] AID_SIG = new byte[] { 0xA0, 0x00, 0x00, 0x01, 0x18, 0x45, 0x43 };
        private static readonly byte[] AID_DEC = new byte[] { 0xA0, 0x00, 0x00, 0x01, 0x18, 0x45, 0x4E };
        private static readonly byte[] DF_SIG = new byte[] { 0xDF, 0x70 };
        private static readonly byte[] DF_DEC = new byte[] { 0xDF, 0x71 };
        private static readonly byte[] EF_C_CH_DS = new byte[] { 0xc0, 0x02 };
        private static readonly byte[] EF_CIN_CSN = new byte[] { 0xD0, 0x01 };
        private static readonly byte[] TLV = new byte[] { 0x84, 0x01, 0x88, 0x80, 0x01, 0x44 };

        public SmardCard_ACOS(IsoReader toSet)
            : base(toSet) {

            if (applicationsMissing()) {
                throw new Exception("Wrong card");
            }
        }

        public override void prepareSignature() {
            sendSelectFID(MASTER_FILE);
            sendSelectFID(DF_SIG);
            Response response2 = sendCase3APDU(isoReader, 0x00, 0x22, 0x41, 0xb6, TLV);
        }

        public override byte[] sign(String pin, byte[] SHA256HASH) {
            byte[] formatedPIN = CashRegisterUtil.getFormat1PIN(pin);
            prepareSignature();
            Response response3 = sendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x81, formatedPIN);
            Response response4 = sendCase3APDU(isoReader, 0x00, 0x2A, 0x90, 0x81, SHA256HASH);
            Response response5 = sendCase2APDU(isoReader, 0x00, 0x2A, 0x9E, 0x9A);
            byte[] signature = response5.GetData();
            return signature;
        }

        public override byte[] signWithoutSelection(String pin, byte[] SHA256HASH) {
            byte[] formatedPIN = CashRegisterUtil.getFormat1PIN(pin);
            Response response3 = sendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x81, formatedPIN);
            Response response4 = sendCase3APDU(isoReader, 0x00, 0x2A, 0x90, 0x81, SHA256HASH);
            Response response5 = sendCase2APDU(isoReader, 0x00, 0x2A, 0x9E, 0x9A);
            byte[] signature = response5.GetData();
            return signature;
        }

        public override string readCertificateSerialDecimal() {
            BigInteger l = getCertificateSerial(DF_SIG, EF_C_CH_DS);
            string dez = l.ToString();
            return dez;
        }

        public override string readCertificateSerialHex() {
            BigInteger l = getCertificateSerial(DF_SIG, EF_C_CH_DS);
            string hex = l.ToString("X");
            return hex;
        }

        public override X509Certificate2 readCertificate() {
            return getCertificate(DF_SIG, EF_C_CH_DS);
        }

        public override string readCIN() {
            sendSelectFID(MASTER_FILE);
            sendSelectFID(DF_DEC);
            // or by using the long form
            // CommandApdu apdu0 = generateDataAPDU(isoReader, 0x00, 0xA4, 0x00, 0x0C, EF_CIN_CSN);
            // Response response0 = isoReader.Transmit(apdu0);
            // CommandApdu apdu = generateResponseAPDU(isoReader, 0x00, 0xB0, 0x00, 0x00); // plain
            Response response = sendCase2APDU(isoReader, 0x00, 0xB0, 0x86, 0x00); // short id
            byte[] data = response.GetData();
            string cin = CashRegisterUtil.byteArrayToHexString(data);
            return cin;
        }

        private Boolean applicationsMissing() {
            Boolean applicationsMissing = false;
            Response response1 = sendSelectAID(AID_DEC);
            if (response1.StatusWord != 0x9000) {
                applicationsMissing = true;
            }
            Response response2 = sendSelectAID(AID_SIG);
            if (response2.StatusWord != 0x9000) {
                applicationsMissing = true;
            }
            return applicationsMissing;
        }
    }
}
