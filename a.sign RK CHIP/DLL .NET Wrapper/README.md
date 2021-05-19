# .NET WRAPPER Function


Registrierkasse.dll is a .NET Wrapper that loads DLLasignP11.dll and calls its native C functions. The wrapper holds a single class named RKWrapper inside the namespace Registrierkasse:
```csharp
namespace Registrierkasse
{
    public class RKWrapper
    {
        // load and init ASignP11.dll
        public RKWrapper();
 
        // get RK data
        public int GetInfo(out string zdaId, out X509Certificate sigCert, out X509Certificate issCert);
 
        // sign
        public int Sign(byte[] dataToSign, out byte[] signature);
         
        // Unload Crypto Lib (only called once at end of program)
        public static int UnloadCryptoLibrary();
 
        // Error information
        public string LastError;
    }
}
```
The return values of `GetInfo` and `Sign` are specified in the [PKCS#11 standard](https://www.cryptsoft.com/pkcs11doc/STANDARD/pkcs-11v2-20.pdf) (0 = success)
