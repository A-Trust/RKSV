using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Registrierkasse
{
    public class RKWrapper
    {
        private static bool isInitialized = false;
        private static bool is32Bit = (IntPtr.Size == 4);

        public string LastError
        {
            private set;
            get;
        }

        public RKWrapper()
        {
            LoadCryptoLibrary();
        }

        ~RKWrapper()
        {
        }


        public int GetInfo(out string zdaId, out X509Certificate sigCert, out X509Certificate issCert)
        {
            zdaId = "";
            sigCert = new X509Certificate();
            issCert = new X509Certificate();

            int ret = 0x0; 
            if (!isInitialized)
            {
                ret = LoadCryptoLibrary();
            }

            if (ret == 0)
            {

                IntPtr pZdaID = Marshal.AllocHGlobal(3);

                ulong sigCertSerialLen = 50;
                IntPtr pSigCertSerial = Marshal.AllocHGlobal((int)sigCertSerialLen);

                byte[] sigCertBytes = new byte[2048];
                IntPtr pSigCert = Marshal.AllocHGlobal(sigCertBytes.Length);
                ulong sigCertLen = (ulong)sigCertBytes.Length;

                byte[] issCertBytes = new byte[2048];
                IntPtr pIssCert = Marshal.AllocHGlobal(issCertBytes.Length);
                ulong issCertLen = (ulong)issCertBytes.Length;

                try
                {
                    if (is32Bit)
                    {
                        Info32(ref zdaId, ref sigCert, ref issCert, ref ret, pZdaID, sigCertSerialLen, pSigCertSerial, sigCertBytes, pSigCert, sigCertLen, issCertBytes, pIssCert, issCertLen);
                    }
                    else
                    {
                        Info64(ref zdaId, ref sigCert, ref issCert, ref ret, pZdaID, sigCertSerialLen, pSigCertSerial, sigCertBytes, pSigCert, sigCertLen, issCertBytes, pIssCert, issCertLen);
                    }
                }
                catch (Exception ex)
                {
                    ret = 0x6; // Function failed
                    LastError = "GetInfo Exception : " + ex.Message;
                }
                finally
                {
                    // Free memory
                    Marshal.FreeHGlobal(pZdaID);
                    Marshal.FreeHGlobal(pSigCertSerial);
                    Marshal.FreeHGlobal(pSigCert);
                    Marshal.FreeHGlobal(pIssCert);
                }


            }
            return ret;

        }

        private void Info32(ref string zdaId, ref X509Certificate sigCert, ref X509Certificate issCert, ref int ret, IntPtr pZdaID, ulong sigCertSerialLen, IntPtr pSigCertSerial, byte[] sigCertBytes, IntPtr pSigCert, ulong sigCertLen, byte[] issCertBytes, IntPtr pIssCert, ulong issCertLen)
        {
            UInt32 sigCertSerialLen32 = (UInt32)sigCertSerialLen;
            UInt32 sigCertLen32 = (UInt32)sigCertLen;
            UInt32 issCertLen32 = (UInt32)issCertLen;

            ret = InternalWrapper.C_RKInfo32(pZdaID, pSigCertSerial, ref sigCertSerialLen32, pSigCert, ref sigCertLen32, pIssCert, ref issCertLen32);

            if (ret == 0)
            {
                byte[] zdaIDbytes = new byte[3];
                // Set data from response
                Marshal.Copy(pZdaID, zdaIDbytes, 0, 3);
                zdaId = System.Text.Encoding.UTF8.GetString(zdaIDbytes);

                Marshal.Copy(pSigCert, sigCertBytes, 0, (int)sigCertLen32);
                Marshal.Copy(pIssCert, issCertBytes, 0, (int)issCertLen32);

                sigCert = new X509Certificate(sigCertBytes);
                issCert = new X509Certificate(issCertBytes);

                LastError = "";
            }
            else
            {
                LastError = "GetInfo: call to C_RKInfo failed";
            }
        }

        private void Info64(ref string zdaId, ref X509Certificate sigCert, ref X509Certificate issCert, ref int ret, IntPtr pZdaID, ulong sigCertSerialLen, IntPtr pSigCertSerial, byte[] sigCertBytes, IntPtr pSigCert, ulong sigCertLen, byte[] issCertBytes, IntPtr pIssCert, ulong issCertLen)
        {
            UInt64 sigCertSerialLen64 = (UInt64)sigCertSerialLen;
            UInt64 sigCertLen64 = (UInt64)sigCertLen;
            UInt64 issCertLen64 = (UInt64)issCertLen;

            ret = InternalWrapper.C_RKInfo64(pZdaID, pSigCertSerial, ref sigCertSerialLen64, pSigCert, ref sigCertLen64, pIssCert, ref issCertLen64);

            if (ret == 0)
            {
                byte[] zdaIDbytes = new byte[3];
                // Set data from response
                Marshal.Copy(pZdaID, zdaIDbytes, 0, 3);
                zdaId = System.Text.Encoding.UTF8.GetString(zdaIDbytes);

                Marshal.Copy(pSigCert, sigCertBytes, 0, (int)sigCertLen64);
                Marshal.Copy(pIssCert, issCertBytes, 0, (int)issCertLen64);

                sigCert = new X509Certificate(sigCertBytes);
                issCert = new X509Certificate(issCertBytes);

                LastError = "";
            }
            else
            {
                LastError = "GetInfo: call to C_RKInfo failed";
            }
        }

        public int Sign(byte[] dataToSign, out byte[] signature)
        {
            signature = null;
            int ret = 0x0; // CKR_OK
            if (!isInitialized)
            {
                ret = LoadCryptoLibrary();
            }


            if (ret == 0)
            {
                IntPtr pSig = IntPtr.Zero;
                IntPtr pTBS = IntPtr.Zero;

                try
                {
                    ulong tbsLen = (ulong)dataToSign.Length;
                    ulong sigLen = 256;

                    pSig = Marshal.AllocHGlobal(256);
                    pTBS = Marshal.AllocHGlobal(dataToSign.Length);

                    if (pSig == IntPtr.Zero || pTBS == IntPtr.Zero)
                    {
                        LastError = "Sign: memory allocation failed";
                        ret = 0x06;
                    }
                    else
                    {
                        Marshal.Copy(dataToSign, 0, pTBS, dataToSign.Length);

                        if (is32Bit)
                        {
                            Sign32(ref signature, ref ret, pSig, pTBS, tbsLen, sigLen);
                        }
                        else
                        {
                            Sign64(ref signature, ref ret, pSig, pTBS, tbsLen, sigLen);
                        }
                    }


                }
                catch (Exception ex)
                {
                    ret = 0x6; // Function failed
                    LastError = "Sign Exception : " + ex.Message;
                }
                finally
                {
                    // Free memory
                    Marshal.FreeHGlobal(pSig);
                }
            }

            return ret;
        }

        private void Sign32(ref byte[] signature, ref int ret, IntPtr pSig, IntPtr pTBS, ulong tbsLen, ulong sigLen)
        {
            UInt32 uSigLen = (UInt32)sigLen;

            ret = InternalWrapper.C_RKSign32(pTBS, (UInt32)tbsLen, pSig, ref uSigLen);
            if (ret == 0)
            {
                signature = new byte[uSigLen];
                Marshal.Copy(pSig, signature, 0, (int)uSigLen);
            }
            else
            {
                LastError = "Sign: call to C_RKSign failed";
            }
        }

        private void Sign64(ref byte[] signature, ref int ret, IntPtr pSig, IntPtr pTBS, ulong tbsLen, ulong sigLen)
        {
            UInt64 uSigLen = (UInt64)sigLen;

            ret = InternalWrapper.C_RKSign64(pTBS, (UInt64)tbsLen, pSig, ref uSigLen);
            if (ret == 0)
            {
                signature = new byte[uSigLen];
                Marshal.Copy(pSig, signature, 0, (int)uSigLen);
            }
            else
            {
                LastError = "Sign: call to C_RKSign failed";
            }
        }


        public int LoadCryptoLibrary()
        {
            int ret = InternalWrapper.C_Initialize(IntPtr.Zero);
            if (ret == 0 // CKR_OK
                || ret == 0x91 // CKR_CRYPTOKI_ALREADY_INITIALIZED
                )
            {
                isInitialized = true;
                ret = 0;
            }
            else
            {
                LastError = "LoadCryptoLibrary failed";
            }

            return ret;
        }

        public static int UnloadCryptoLibrary()
        {
            int ret = InternalWrapper.C_Finalize(IntPtr.Zero);
            if (ret == 0 // CKR_OK
                || ret == 0x90// CKR_CRYPTOKI_NOT_INITIALIZED
                )
            {
                isInitialized = false;
                ret = 0;
            }

            return ret;
        }



    }
}
