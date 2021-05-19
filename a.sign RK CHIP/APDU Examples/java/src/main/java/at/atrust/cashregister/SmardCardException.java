package at.atrust.cashregister;

/**
 * Created by chinnow on 16.10.2015.
 */
public class SmardCardException extends Exception {

	private static final long serialVersionUID = 5377974919590757907L;

	public SmardCardException(Exception e) {
		super(e);
	}

	public SmardCardException(String message) {
		super(message);
	}
}
