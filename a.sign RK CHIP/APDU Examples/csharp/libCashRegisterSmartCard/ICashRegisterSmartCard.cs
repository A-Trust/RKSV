namespace Cashregister.Smartcard {

    using System;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Interface for cash register smart cards</summary>
    public interface ICashRegisterSmartCard {

        /// <summary>
        /// Gets the serial of the certificate on the cash register smartcard</summary>
        /// <returns>
        /// Returns the serial in decimal</returns>
        string readCertificateSerialDecimal();

        /// <summary>
        /// Gets the serial of the certificate on the cash register smartcard</summary>
        /// <returns>
        /// Returns the serial in hexadecimal</returns>
        string readCertificateSerialHex();

        /// <summary>
        /// Gets the certificate which belongs to the cash register smartcard</summary>
        /// <remarks>
        /// Reading the whole certificate is slow. If you need only the serial use
        /// getCertificateSerialDecimal() or getCertificateSerialHex()</remarks>
        /// <returns>
        /// Returns the certificate as X509Certificate2</returns>
        X509Certificate2 readCertificate();

        /// <summary>
        /// Perpares the smart card for signing data</summary>
        /// <remarks>
        /// If you want to sign multiple hashes one after another, you only have to call this
        /// function once to set the neccessary parameter</remarks>
        void prepareSignature();

        /// <summary>
        /// Signs the given hash.</summary>
        /// <remarks>
        /// The function is slower than signWithoutSelection() but save, as all 
        /// required parameter are set before the signature</remarks>
        byte[] sign(String pin, byte[] SHA256HASH);

        /// <summary>
        /// Signs the given hash. Make sure that prepareSignature() is called before.</summary>
        /// <remarks>
        /// If you want to sign multiple hashes one after another, you have to call the
        /// prepareSignature() function only once to set the neccessary parameter</remarks>
        byte[] signWithoutSelection(String pin, byte[] SHA256HASH);

        /// <summary>
        /// Verifies that the given signature is correct for the given signed data</summary>
        bool verify(byte[] signature, byte[] signedData);

        /// <summary>
        /// Reads the CIN of the smart card</summary>
        string readCIN();
    }
}
