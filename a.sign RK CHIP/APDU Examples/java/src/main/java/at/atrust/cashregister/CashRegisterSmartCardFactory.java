package at.atrust.cashregister;

import javax.smartcardio.ATR;
import javax.smartcardio.Card;

/**
 * Created by chinnow on 03.05.2016.
 */
public class CashRegisterSmartCardFactory {
	
    // Obsolete ATRs for cards not certified for use after 07.06.2025
    private static final String ACOSID_ATR_V2_COLD = "3BDF96FF910131FE4680319052410264050200AC73D622C017";
    private static final String ACOSID_ATR_V2_WARM = "3BDF18FF910131FE4680319052410264050200AC73D622C099";
    private static final String ACOSID_ATR_V3 = "3BDF97008131FE4680319052410364050201AC73D622C0F8";
    // ATR for the newest ACOS card
    private static final String ACOSID_ATR_V4_1 = "3BDF97008131FE4680319052410464050401AC73D622C0F9";
	
	public static ICashRegisterSmartCard createInstance(Card card) throws SmardCardException {
		ICashRegisterSmartCard toReturn;
		ATR atr = card.getATR();
		String atrHex = SmartCardUtil.byteArrayToHexString(atr.getBytes());
		if (atrHex.startsWith("3BBF11008131FE45455041")) {
			toReturn = new SmartCardACOS(card);
		} else if (atrHex.startsWith("3BBF11008131FE454D4341")) {
			toReturn = new SmartCardACOS(card);
		} else if (atrHex.startsWith("3BDF18008131FE588031B05202046405C903AC73B7B1D422")) {
			toReturn = new SmartCardCardOS(card);
		} else if (atrHex.startsWith("3BDF18008131FE588031905241016405C903AC73B7B1D444")) {
			toReturn = new SmartCardCardOS(card);
		} else if (atrHex.startsWith(ACOSID_ATR_V2_COLD)) {
			toReturn = new SmartCardAcosID(card);
		} else if (atrHex.startsWith(ACOSID_ATR_V2_WARM)) {
			toReturn = new SmartCardAcosID(card);
		} else if (atrHex.startsWith(ACOSID_ATR_V3)) {
			toReturn = new SmartCardAcosID(card);
		} else if (atrHex.startsWith(ACOSID_ATR_V4_1)) {
			toReturn = new SmartCardAcosID(card);
		} else {
			throw new SmardCardException("Wrong card");
		}
		return toReturn;
	}

}
