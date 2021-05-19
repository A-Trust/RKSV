Imports Registrierkasse.RKWrapper
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Module Module1

    Sub Main()

        Dim rkw As New Registrierkasse.RKWrapper

        Dim ret As Integer
        Dim zdaId As String
        Dim sigCertSerial As String
        Dim sigCert As X509Certificate
        Dim issuerCert As X509Certificate

        ret = rkw.GetInfo(zdaId, sigCert, issuerCert)

        If ret = 0 Then
            sigCertSerial = sigCert.GetSerialNumberString()
            Console.WriteLine("ZDA ID = " + zdaId)
            Console.WriteLine("Sig Cert Serial = " + sigCertSerial)

            Dim sigCertBytes As Byte()
            sigCertBytes = sigCert.Export(X509ContentType.Cert)
            Console.WriteLine("Sig Cert B64:    " + Convert.ToBase64String(sigCertBytes))

            Dim issCertBytes As Byte()
            issCertBytes = issuerCert.Export(X509ContentType.Cert)
            Console.WriteLine("Issuer Cert B64: " + Convert.ToBase64String(issCertBytes))
        Else
            Console.WriteLine("GetInfo failed with return code = 0x" + ret.ToString("X4"))
            Console.WriteLine("Last error : " + rkw.LastError)
        End If

        Dim toBeSignedStr As String
        Dim JWS_Protected_Header As String
        Dim JWS_Payload As String
        Dim tbsBytes As Byte()
        Dim signature As Byte()

        JWS_Protected_Header = Base64url("{""alg"":""ES256""}")
        JWS_Payload = Base64url("_R1-AT1_DEMO-CASH-BOX426_776740_2015-10-14T18:20:23_53,45_6,61_0,00_47,65_3,33_NGGrKn6Kq3c=_8645153189157718879_bacPpqE/lw4=")
        toBeSignedStr = String.Format("{0}.{1}", JWS_Protected_Header, JWS_Payload)
        tbsBytes = System.Text.Encoding.UTF8.GetBytes(toBeSignedStr)

        ret = rkw.Sign(tbsBytes, signature)

        If ret = 0 Then
            Dim sigB64 As String
            sigB64 = Base64url(signature)
            Console.WriteLine("Signature:    " + sigB64)
        Else
            Console.WriteLine("Sign failed with return code = 0x" + ret.ToString("X4"))
            Console.WriteLine("Last error : " + rkw.LastError)
        End If

        Registrierkasse.RKWrapper.UnloadCryptoLibrary()


        Console.ReadLine()

    End Sub

    Function Base64url(ByVal input As String) As String
        Return Base64url(System.Text.Encoding.UTF8.GetBytes(input))
    End Function

    Function Base64url(ByVal inputBytes As Byte()) As String
        Dim result As New StringBuilder(Convert.ToBase64String(inputBytes).TrimEnd("="))
        result.Replace("+", "-")
        result.Replace("/", "_")
        Return result.ToString()
    End Function


End Module
