namespace Cashregister.Smartcard {

    using System.Numerics;
    using System;
    using PCSC.Iso7816;
    using System.Security.Cryptography.X509Certificates;

    class SmartCard_CardOS_5_3 : AbstractCashRegisterSmardCard {

        private static readonly byte[] DF_SIG = new byte[] { 0xDF, 0x01 };

        private static readonly byte[] EF_CIN_CSN = new byte[] { 0xD0, 0x01 };
        private static readonly byte[] EF_C_CH_DS = new byte[] { 0xc0, 0x00 };

        private static readonly byte[] AID_SIG = new byte[] { 0xD0, 0x40, 0x00, 0x00, 0x22, 0x00, 0x01 };

        public SmartCard_CardOS_5_3(IsoReader toSet)
            : base(toSet) {
            if (applicationsMissing()) {
                throw new Exception("Wrong card");
            }
        }

        public override void prepareSignature() {
            Response selectMFResponse = sendSelectFID(MASTER_FILE);
            Response selectDFResponse = sendSelectFID(DF_SIG);
        }

        public override byte[] sign(String pin, byte[] SHA256HASH) {
            byte[] formatedPIN = CashRegisterUtil.getFormat2PIN(pin);
            prepareSignature();
            Response response3 = sendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x81, formatedPIN);
            Response responseSign = sendCase4APDU(0x00, 0x2A, 0x9E, 0x9A, SHA256HASH, 64);
            if (responseSign.StatusWord != 0x9000) {
                throw new Exception();
            }
            byte[] signature = responseSign.GetData();
            return signature;
        }

        public override byte[] signWithoutSelection(String pin, byte[] SHA256HASH) {
            byte[] formatedPIN = CashRegisterUtil.getFormat2PIN(pin);
            Response response3 = sendCase3APDU(isoReader, 0x00, 0x20, 0x00, 0x81, formatedPIN);
            Response responseSign = sendCase4APDU(0x00, 0x2A, 0x9E, 0x9A, SHA256HASH, 64);
            if (responseSign.StatusWord != 0x9000) {
                throw new Exception();
            }
            byte[] signature = responseSign.GetData();
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
            Response selectDFResponse = sendSelectFID(MASTER_FILE);
            Response selectEFResponse = sendSelectFID(EF_CIN_CSN);
            Response response = sendCase2APDU(isoReader, 0x00, 0xB0, 0x00, 0x00); // plain
            byte[] data = response.GetData();
            string cin = CashRegisterUtil.byteArrayToHexString(data);
            return cin;
        }

        private Boolean applicationsMissing() {
            Boolean applicationsMissing = false;
            Response response2 = sendSelectAID(AID_SIG);
            if (response2.StatusWord != 0x9000) {
                applicationsMissing = true;
            }
            return applicationsMissing;
        }
    }
}
