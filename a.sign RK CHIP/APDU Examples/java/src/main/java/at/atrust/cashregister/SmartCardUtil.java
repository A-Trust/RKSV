package at.atrust.cashregister;

import java.io.ByteArrayInputStream;
import java.security.cert.Certificate;
import java.security.cert.CertificateFactory;
import java.util.List;

import org.bouncycastle.cert.X509CertificateHolder;

/**
 * Created by chinnow on 03.05.2016.
 */
public class SmartCardUtil {

	public static String byteArrayToHexString(byte[] data) {
		String cin;
		final StringBuilder builder = new StringBuilder(8);
		for (byte b : data) {
			builder.append(String.format("%02x", b));
		}
		cin = builder.toString();
		return cin.toUpperCase();
	}

	public static int byteToUnsignedint(byte b) {
		int i = b;
		if (i < 0) {
			i += 256;
		}
		return i;
	}

	public static X509CertificateHolder buildX509Certificate(List<byte[]> dataList) throws SmardCardException {

		int size = (dataList.size() - 1) * 256;
		size += dataList.get(dataList.size() - 1).length;
		byte[] cert = new byte[size];
		int i = 0;
		for (byte[] b : dataList) {
			System.arraycopy(b, 0, cert, i, b.length);
			i += b.length;
		}
		try {
			X509CertificateHolder ch = new X509CertificateHolder(cert);
			return ch;
		} catch (Exception e) {
			e.printStackTrace();
			try {
				ByteArrayInputStream bis = new ByteArrayInputStream(cert);
				Certificate c2 = CertificateFactory.getInstance("X509").generateCertificate(bis);
				X509CertificateHolder ch = new X509CertificateHolder(c2.getEncoded());
				return ch;
			} catch (Exception e2) {
				e2.printStackTrace();
				throw new SmardCardException("Error building certificate");
			}
		}
		/*
		 * catch (Exception e) { e.printStackTrace(); throw new
		 * SmardCardException("Error building certificate"); }
		 */
	}

	public static byte[] getFormat1PIN(String pin) throws SmardCardException {
		if (pin.length() != 6 && pin.length() != 4) {
			throw new SmardCardException("Wrong PIN length");
		}
		char[] ca = pin.toCharArray();
		byte[] ba = new byte[8];
		for (int i = 0; i < 8; i++) {
			if (i < ca.length) {
				ba[i] = (byte) ca[i];
			} else {
				ba[i] = 0x00;
			}
		}
		return ba;
	}

	/**
	 * Format 2 PIN block The format 2 PIN block is constructed thus: 1 nibble
	 * with the value of 2, which identifies this as a format 2 block 1 nibble
	 * encoding the length N of the PIN N nibbles, each encoding one PIN digit
	 * 14-N nibbles, each holding the "fill" value 15
	 */
	public static byte[] getFormat2PIN(String pin) throws SmardCardException {

		if (pin.length() != 6 && pin.length() != 4) {
			throw new SmardCardException("Wrong PIN length");
		}
		byte[] ba = new byte[8];
		ba[0] = (byte) ((2 << 4) | pin.length());
		char[] ca = pin.toCharArray();
		ba[1] = (byte) (((ca[0] - 0x30) << 4) | (ca[1] - 0x30));
		ba[2] = (byte) (((ca[2] - 0x30) << 4) | (ca[3] - 0x30));
		if (pin.length() == 6) {
			ba[3] = (byte) (((ca[4] - 0x30) << 4) | (ca[5] - 0x30));
		} else {
			ba[3] = (byte) 0xFF;
		}
		ba[4] = (byte) 0xFF;
		ba[5] = (byte) 0xFF;
		ba[6] = (byte) 0xFF;
		ba[7] = (byte) 0xFF;
		return ba;
	}
}
