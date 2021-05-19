package at.atrust.cashregister;

import javax.smartcardio.ATR;
import javax.smartcardio.Card;

/**
 * Created by chinnow on 03.05.2016.
 */
public class CashRegisterSmartCardFactory {

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
		} else {
			throw new SmardCardException("Wrong card");
		}
		return toReturn;
	}

}
