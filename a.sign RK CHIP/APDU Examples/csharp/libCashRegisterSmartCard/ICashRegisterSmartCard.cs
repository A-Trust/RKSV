namespace Cashregister.Smartcard
{

    using System;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Interface for cash register smart cards</summary>
    public interface ICashRegisterSmartCard
    {

        /// <summary>
        /// Gets the serial of the certificate on the cash register smartcard</summary>
        /// <returns>
        /// Returns the serial in decimal</returns>
        string ReadCertificateSerialDecimal();

        /// <summary>
        /// Gets the serial of the certificate on the cash register smartcard</summary>
        /// <returns>
        /// Returns the serial in hexadecimal</returns>
        string ReadCertificateSerialHex();

        /// <summary>
        /// Gets the certificate which belongs to the cash register smartcard</summary>
        /// <remarks>
        /// Reading the whole certificate is slow. If you need only the serial use
        /// getCertificateSerialDecimal() or getCertificateSerialHex()</remarks>
        /// <returns>
        /// Returns the certificate as X509Certificate2</returns>
        X509Certificate2 ReadCertificate();

        /// <summary>
        /// Perpares the smart card for signing data</summary>
        /// <remarks>
        /// If you want to sign multiple hashes one after another, you only have to call this
        /// function once to set the neccessary parameter</remarks>
        void PrepareSignature();

        /// <summary>
        /// Signs the given hash.</summary>
        /// <remarks>
        /// The function is slower than signWithoutSelection() but save, as all 
        /// required parameter are set before the signature</remarks>
        byte[] Sign(String pin, byte[] SHA256HASH);

        /// <summary>
        /// Signs the given hash. Make sure that prepareSignature() is called before.</summary>
        /// <remarks>
        /// If you want to sign multiple hashes one after another, you have to call the
        /// prepareSignature() function only once to set the neccessary parameter</remarks>
        byte[] SignWithoutSelection(String pin, byte[] SHA256HASH);

        /// <summary>
        /// Verifies that the given signature is correct for the given signed data</summary>
        bool Verify(byte[] signature, byte[] signedData);

        /// <summary>
        /// Reads the CIN of the smart card</summary>
        string ReadCIN();
    }
}
