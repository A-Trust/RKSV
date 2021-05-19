package at.atrust.cashregister;

import java.util.ArrayList;
import java.util.List;

import javax.smartcardio.CardChannel;
import javax.smartcardio.CardException;
import javax.smartcardio.CommandAPDU;
import javax.smartcardio.ResponseAPDU;

/**
 * Created by chinnow on 03.05.2016.
 */
public abstract class AbstractCashRegisterSmartCard implements ICashRegisterSmartCard {
	
	protected static final byte[] MASTER_FILE = new byte[] { 0x3F, 0x00 };

	protected CardChannel channel;

	protected void executeSelectWithFileIdAPDU(byte[] fileID) throws SmardCardException {

		CommandAPDU select = new CommandAPDU(0x00, 0xA4, 0x00, 0x0C, fileID);
		executeCommand(select);
	}

	protected ResponseAPDU selectWithAppliactionId(byte[] appliactionId) throws SmardCardException {
		CommandAPDU select = new CommandAPDU(0x00, 0xA4, 0x04, 0x0C, appliactionId);
		return executeCommand(select);
	}

	protected ResponseAPDU executeCommand(CommandAPDU commandAPDU) throws SmardCardException {

		ResponseAPDU responseAPDU = null;
		try {
			responseAPDU = channel.transmit(commandAPDU);
			if (responseAPDU.getSW() != 0x9000 && responseAPDU.getSW() != 0x6A82) {
				throw new SmardCardException("Response APDU status is " + responseAPDU.getSW());
			}
		} catch (Exception e) {
			throw new SmardCardException(e);
		}
		return responseAPDU;
	}

	protected byte[] getData(CommandAPDU commandAPDU) throws SmardCardException {

		try {
			ResponseAPDU responseAPDU = channel.transmit(commandAPDU);
			if (responseAPDU.getSW() != 0x9000) {
				throw new SmardCardException("Response APDU status is " + responseAPDU.getSW());
			} else {
				return responseAPDU.getData();
			}
		} catch (Exception e) {
			throw new SmardCardException(e);
		}
	}

	protected List<byte[]> getBuffer(boolean onlyFirst, byte[] DF, byte[] EF) throws SmardCardException, CardException {

		executeSelectWithFileIdAPDU(MASTER_FILE);
		executeSelectWithFileIdAPDU(DF);
		executeSelectWithFileIdAPDU(EF);
		int offset = 0;
		List<byte[]> dataList = new ArrayList<>(8);
		while (true) {
			ResponseAPDU resp = channel.transmit(new CommandAPDU(0x00, 0xB0, 0x7F & (offset >> 8), offset & 0xFF, 256));
			if (resp.getSW() != 0x9000) {
				break;
			}
			dataList.add(resp.getData());
			if (onlyFirst) {
				break;
			}
			offset += 256;
		}
		return dataList;
	}

}
