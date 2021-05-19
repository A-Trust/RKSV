using System;
using System.Runtime.InteropServices;

namespace Registrierkasse
{
    class InternalWrapper
    {
        const string LIBPATH = "asignp11.dll";


        //CK_DEFINE_FUNCTION(CK_RV, C_Initialize)(
        //CK_VOID_PTR pInitArgs
        //);
        [DllImport(LIBPATH, EntryPoint = "C_Initialize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_Initialize(IntPtr args);


        //CK_DEFINE_FUNCTION(CK_RV, C_Finalize)(
        //CK_VOID_PTR pReserved
        //);
        [DllImport(LIBPATH, EntryPoint = "C_Finalize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_Finalize(IntPtr args);

        //CK_DEFINE_FUNCTION(CK_RV, C_RKInfo)(
        //    CK_UTF8CHAR ZdaId[3],
        //    CK_BYTE_PTR pSigCertSerial,
        //    CK_ULONG_PTR pulSigCertSerialLen,
        //    CK_BYTE_PTR pSigCert,
        //    CK_ULONG_PTR pulSigCertLen,
        //    CK_BYTE_PTR pIssuerCert,
        //    CK_ULONG_PTR pulIssuerCertLen
        //);
        [DllImport(LIBPATH, EntryPoint = "C_RKInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_RKInfo64(IntPtr zdaId, 
            IntPtr SigCertSerial, 
            ref UInt64 SigCertSerialLen,
            IntPtr SigCert,
            ref UInt64 SigCertLen,
            IntPtr IssuerCert,
            ref UInt64 IssuerCertLen);

        [DllImport(LIBPATH, EntryPoint = "C_RKInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_RKInfo32(IntPtr zdaId,
            IntPtr SigCertSerial,
            ref UInt32 SigCertSerialLen,
            IntPtr SigCert,
            ref UInt32 SigCertLen,
            IntPtr IssuerCert,
            ref UInt32 IssuerCertLen);


        //CK_DEFINE_FUNCTION(CK_RV, C_RKSign)(
        //    CK_BYTE_PTR pData,
        //    CK_ULONG ulData,
        //    CK_BYTE_PTR pSignature,
        //    CK_ULONG_PTR pulSignatureLen
        //);
        [DllImport(LIBPATH, EntryPoint = "C_RKSign", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_RKSign64(IntPtr tbsData,
            UInt64 tbsDataLen,
            IntPtr signature,
            ref UInt64 signatureLen);

        [DllImport(LIBPATH, EntryPoint = "C_RKSign", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_RKSign32(IntPtr tbsData,
            UInt32 tbsDataLen,
            IntPtr signature,
            ref UInt32 signatureLen);

    }
}
