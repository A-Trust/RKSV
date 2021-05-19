package at.atrust.cashregister;

import javax.smartcardio.CardException;

import org.bouncycastle.cert.X509CertificateHolder;

/**
 * Created by chinnow on 03.05.2016.
 */
public interface ICashRegisterSmartCard {

	byte[] doSignatur(byte[] sha256Hash, String pin) throws SmardCardException;

	byte[] doSignaturWithoutSelection(byte[] sha256Hash, String pin) throws SmardCardException;

	String getCertificateSerialDecimal() throws SmardCardException, CardException;

	String getCertificateSerialHex() throws SmardCardException, CardException;

	X509CertificateHolder getCertificate() throws SmardCardException, CardException;

	String getCIN() throws SmardCardException, CardException;

}
