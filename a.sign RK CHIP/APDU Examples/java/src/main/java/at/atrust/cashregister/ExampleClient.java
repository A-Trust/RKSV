package at.atrust.cashregister;

import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.List;

import javax.smartcardio.Card;
import javax.smartcardio.CardException;
import javax.smartcardio.CardTerminal;
import javax.smartcardio.CardTerminals;
import javax.smartcardio.TerminalFactory;

import org.bouncycastle.cert.X509CertificateHolder;

/**
 * Created by chinnow on 21.10.2015.
 */
public class ExampleClient {

	public static void main(String[] a) {

		try {
			List<Card> cards = getAttachedCards();
			Card myCard = cards.get(0);
			ICashRegisterSmartCard cashRegisterSmartCard = CashRegisterSmartCardFactory.createInstance(myCard);
			String cin = cashRegisterSmartCard.getCIN();
			System.out.println("I'm using card with cin " + cin);
			X509CertificateHolder cert = cashRegisterSmartCard.getCertificate();
			System.out.println("I'm using cert with Subject " + cert.getSubject().toString());
			String certSerialDec = cashRegisterSmartCard.getCertificateSerialDecimal();
			System.out.println("I'm using cert with ID (dec) " + certSerialDec);
			String certSerialHex = cashRegisterSmartCard.getCertificateSerialHex();
			System.out.println("I'm using cert with ID (hex) " + certSerialHex);

			byte[] exampleSha256Hash = { (byte) 0xe3, (byte) 0xb0, (byte) 0xc4, 0x42, (byte) 0x98, (byte) 0xfc, 0x1c,
					0x14, (byte) 0x9a, (byte) 0xfb, (byte) 0xf4, (byte) 0xc8, (byte) 0x99, 0x6f, (byte) 0xb9, 0x24,
					0x27, (byte) 0xae, 0x41, (byte) 0xe4, 0x64, (byte) 0x9b, (byte) 0x93, 0x4c, (byte) 0xa4,
					(byte) 0x95, (byte) 0x99, 0x1b, 0x78, 0x52, (byte) 0xb8, 0x55 };
			String exampleCardPIN = "123456";

			long startTime = System.currentTimeMillis();
			int numberOfSignatures = 10;
			byte[] exampleSignature = cashRegisterSmartCard.doSignatur(exampleSha256Hash, exampleCardPIN);
			for (int i = 1; i <= numberOfSignatures; i++) {
				exampleSignature = cashRegisterSmartCard.doSignaturWithoutSelection(exampleSha256Hash, exampleCardPIN);
				System.out.println("Signature " + i + " / " + numberOfSignatures);
			}
			long endTime = System.currentTimeMillis();
			long duration = endTime - startTime;
			System.out.println("Needed " + duration / 1000.0 + " seconds for " + numberOfSignatures + " signatures ");
			System.out.println("That's  " + duration / 1000.0 / numberOfSignatures + " seconds per signature");
			System.out.println("===");
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private static List<Card> getAttachedCards() {
		TerminalFactory terminalFactory;
		try {
			terminalFactory = TerminalFactory.getInstance("PC/SC", null);
		} catch (NoSuchAlgorithmException e) {
			terminalFactory = TerminalFactory.getDefault();
		}
		CardTerminals cardTerminals = terminalFactory.terminals();
		List<Card> cards = new ArrayList<>(5);
		try {
			for (CardTerminal terminal : cardTerminals.list(CardTerminals.State.CARD_INSERTION)) {
				try {
					Card card = terminal.connect("*");
					cards.add(card);
				} catch (CardException e) {
					e.printStackTrace();
				}
			}
		} catch (CardException e) {
			e.printStackTrace();
		}
		return cards;
	}
}
