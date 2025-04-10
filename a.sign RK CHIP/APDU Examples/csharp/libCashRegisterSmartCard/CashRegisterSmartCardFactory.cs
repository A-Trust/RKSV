﻿namespace Cashregister.Smartcard
{

    using System;
    using PCSC;
    using PCSC.Iso7816;

    public sealed class CashRegisterSmartCardFactory
    {
        // Obsolete ATRs for cards not certified for use after 07.06.2025
        private const string ACOSID_ATR_V2_COLD = "3BDF96FF910131FE4680319052410264050200AC73D622C017";
        private const string ACOSID_ATR_V2_WARM = "3BDF18FF910131FE4680319052410264050200AC73D622C099";
        private const string ACOSID_ATR_V3 = "3BDF97008131FE4680319052410364050201AC73D622C0F8";
        // ATR for the newest ACOS card
        private const string ACOSID_ATR_V4_1 = "3BDF97008131FE4680319052410464050401AC73D622C0F9";

        private CashRegisterSmartCardFactory()
        {
            // no instances
        }

        public static ICashRegisterSmartCard CreateInstance(SCardContext context, string readerName)
        {
            string atrHex = GetATR(context, readerName);
            IsoReader isoReader = new IsoReader(context, readerName, SCardShareMode.Shared, SCardProtocol.Any, false);
            if (isoReader == null)
            {
                throw new Exception("Reader is null");
            }
            ICashRegisterSmartCard cardInstance;
            if (atrHex.StartsWith("3BBF11008131FE45455041"))
            {
                cardInstance = new SmardCard_ACOS(isoReader);
            }
            else if (atrHex.StartsWith("3BBF11008131FE454D4341"))
            {
                cardInstance = new SmardCard_ACOS(isoReader);
            }
            else if (atrHex.StartsWith("3BDF18008131FE588031B05202046405C903AC73B7B1D422"))
            {
                cardInstance = new SmartCard_CardOS_5_3(isoReader);
            }
            else if (atrHex.StartsWith("3BDF18008131FE58803190"))
            {
                cardInstance = new SmartCard_CardOS_5_3(isoReader);
            }
            else if (atrHex.StartsWith(ACOSID_ATR_V2_COLD))
            {
                cardInstance = new SmartCard_AcosID(isoReader);
            }
            else if (atrHex.StartsWith(ACOSID_ATR_V2_WARM))
            {
                cardInstance = new SmartCard_AcosID(isoReader);
            }
            else if (atrHex.StartsWith(ACOSID_ATR_V3)) 
            {
                cardInstance = new SmartCard_AcosID(isoReader);
            }
            else if (atrHex.StartsWith(ACOSID_ATR_V4_1))
            {
                cardInstance = new SmartCard_AcosID(isoReader);
            }
            else
            {
                throw new Exception("Wrong card, ATR " + atrHex + " not supported");
            }
            return cardInstance;
        }

        private static string GetATR(SCardContext context, string readerName)
        {
            string atrHex = null;
            try
            {
                using (ICardReader reader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
                {
                    ReaderStatus state = reader.GetStatus();
                    byte[] atr = state.GetAtr();
                    atrHex = CashRegisterUtil.ByteArrayToHexString(atr);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw new Exception("Can't get ATR");
            }
            if (atrHex == null)
            {
                throw new Exception("Can't get ATR");
            }
            return atrHex;
        }
    }
}
