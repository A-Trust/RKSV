# Examples for Registrierasse DLL
Two new functions have been built into the A-Trust driver DLL.

```cpp
CK_DEFINE_FUNCTION(CK_RV, C_RKInfo)(
    CK_UTF8CHAR ZdaId[3],
    CK_BYTE_PTR pSigCertSerial,
    CK_ULONG_PTR pulSigCertSerialLen,
    CK_BYTE_PTR pSigCert,
    CK_ULONG_PTR pulSigCertLen,
    CK_BYTE_PTR pIssuerCert,
    CK_ULONG_PTR pulIssuerCertLen
);
```

The function returns the following data: ZDA Id, Signature Certificate Serial Number, Signature Certificate, Issuer Certificate.

The values pulSigCertSerialLen, pulZdaIdLen, pulSigCertLen and pulIssuerCertLen must be initialized with the preallocated lengths of pSigCertSerial, pZdaId, pSigCert, and pIssuerCert. They are changed by the function with the real magnitudes. 

To query the correct lengths, one can call the function with pZdaId = pSigCert = pIssuerCert = NULL.

```cpp
CK_DEFINE_FUNCTION(CK_RV, C_RKSign)(
    CK_BYTE_PTR pData,
    CK_ULONG ulData,
    CK_BYTE_PTR pSignature,
    CK_ULONG_PTR pulSignatureLen
);
```

The function tries to create a session for the signature key and signs the data in pData. If pSignature is set to NULL_PTR, the function returns only the length of the signature in pulSignaturLen.