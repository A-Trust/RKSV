// RegistrierkasseP11.cpp : Defines the entry point for the console application.
//
 
#include "stdafx.h"
#include <Windows.h>
#include "p11Functions.h"
#include <string>
#include <vector>
 
const char* PKCS11_LIB_NAME = "asignp11.dll";
static HMODULE m_lib = NULL;
static bool isInitialized = false;
 
typedef CK_DEFINE_FUNCTION(CK_RV, Type_C_RKSign)(
    CK_BYTE_PTR toBeSigned,
    CK_ULONG ulToBeSignedLen,
    CK_BYTE_PTR signature,
    CK_ULONG_PTR pulSignature
    );
Type_C_RKSign* Proc_C_RKSign = NULL;
 
typedef CK_DEFINE_FUNCTION(CK_RV, Type_C_RKInfo)(
    CK_UTF8CHAR ZdaId[3],
    CK_BYTE_PTR pSigCertSerial,
    CK_ULONG_PTR pulSigCertSerialLen,
    CK_BYTE_PTR pSigCert,
    CK_ULONG_PTR pulSigCertLen,
    CK_BYTE_PTR pIssuerCert,
    CK_ULONG_PTR pulIssuerCertLen
    );
Type_C_RKInfo* Proc_C_RKInfo = NULL;
 
 
 
bool initLib()
{
    // This should only run once
    if (isInitialized)
        return true;
 
    m_lib = LoadLibraryA(PKCS11_LIB_NAME);
    if (m_lib == NULL)
    {
        return false;
    }
 
    Proc_C_Initialize = (Type_C_Initialize *)GetProcAddress(m_lib, "C_Initialize");
    if (NULL == Proc_C_Initialize)
    {
        return false;
    }
    Proc_C_Finalize = (Type_C_Finalize *)GetProcAddress(m_lib, "C_Finalize");
    Proc_C_RKSign = (Type_C_RKSign *)GetProcAddress(m_lib, "C_RKSign");
    Proc_C_RKInfo = (Type_C_RKInfo *)GetProcAddress(m_lib, "C_RKInfo");
 
 
    return true;
}
 
void closeLib()
{
    FreeLibrary(m_lib);
}
 
 
int _tmain(int argc, _TCHAR* argv[])
{
 
    //Load library
    if (!initLib())
    {
        exit(-1);
    }
 
    CK_RV ret = Proc_C_Initialize(NULL);
 
    CK_UTF8CHAR ZdaId[3];
    CK_UTF8CHAR pSigCertSerial[32];
    CK_ULONG ulSigCertSerialLen = 32;
 
    CK_BYTE pSigCert[2048];
    CK_ULONG ulSigCertLen = 2048;
    CK_BYTE pIssuerCert[2048];
    CK_ULONG ulIssuerCertLen = 2048;
 
    ret = Proc_C_RKInfo(ZdaId, pSigCertSerial, &ulSigCertSerialLen,
        pSigCert, &ulSigCertLen, pIssuerCert, &ulIssuerCertLen);
 
    if (ret == 0)
    {
        printf("C_RKInfo SUCCESS\r\n");
    }
    else
    {
        printf("C_RKInfo failed with return code %X\r\n", ret);
    }
 
    // Sign the JWS formatted string
 
    // The header ist the Base64Url encoding of the string: {"alg":"ES256"}
    std::string JWS_Protected_Header = "eyJhbGciOiJFUzI1NiJ9";  // = Base64url("{\"alg\":\"ES256\"}");
 
    // The payload is the Base64url encoding of the machine readable format
    std::string JWS_Payload = "X1IxLUFUMF9ERU1PLUNBU0gtQk9YODE3XzgzNDczXzIwMTUtMTEtMjVUMTk6MjA6MTFfMCwwMF8wLDAwXzAsMDBfMCwwMF8wLDAwX2dpc2pxWGFBR1VnPV8tMzY2Nzk2MTg3NTcwNjM1Njg0OV9weFhlTG4vZXhTRT0";
 
    std::string toBeSignedStr = JWS_Protected_Header + "." + JWS_Payload;
    std::string signedData;
    std::string certSerialNum;
 
    CK_ULONG ulSignatureLen = 256;
    CK_BYTE_PTR signature = new CK_BYTE[ulSignatureLen];
 
    ret = Proc_C_RKSign((unsigned char*)toBeSignedStr.data(), toBeSignedStr.size(),
        signature, &ulSignatureLen);
 
    if (ret == 0)
    {
        printf("C_RKSign SUCCESS\r\n");
    }
    else
    {
        printf("C_RKSign failed with return code %X\r\n", ret);
    }
 
    ret = Proc_C_Finalize(NULL);
 
    closeLib();
 
    return 0;
}