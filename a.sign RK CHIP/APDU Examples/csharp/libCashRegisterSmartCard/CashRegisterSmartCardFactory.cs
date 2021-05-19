namespace Cashregister.Smartcard {

    using System;
    using PCSC;
    using PCSC.Iso7816;

    public sealed class CashRegisterSmartCardFactory {

        private CashRegisterSmartCardFactory() {
            // no instances
        }

        public static ICashRegisterSmartCard createInstance(IsoReader isoReader) {

            ICashRegisterSmartCard cardInstance = null;
            if (isoReader == null) {
                throw new Exception("Reader is null");
            }
            SCardReaderState state = isoReader.CurrentContext.GetReaderStatus(isoReader.ReaderName);
            byte[] atr = state.Atr;
            string atrHex = CashRegisterUtil.byteArrayToHexString(atr);
            if (atrHex.StartsWith("3BBF11008131FE45455041")) {
                cardInstance = new SmardCard_ACOS(isoReader);
            } else if (atrHex.StartsWith("3BBF11008131FE454D4341")) {
                cardInstance = new SmardCard_ACOS(isoReader);
            } else if (atrHex.StartsWith("3BDF18008131FE588031B05202046405C903AC73B7B1D422")) {
                // TODO check the required length here // "5A0A80040000002300485112"
                cardInstance = new SmartCard_CardOS_5_3(isoReader);
            } else if (atrHex.StartsWith("3BDF18008131FE58803190")) {
                //                        3BDF18008131FE588031905241016405C903AC73B7B1D444
                cardInstance = new SmartCard_CardOS_5_3(isoReader);
            } else {
                throw new Exception("Wrong card");
            }
            return cardInstance;
        }
    }
}
