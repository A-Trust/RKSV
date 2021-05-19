Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography
 
Module Registrierkasse
 
    Sub Main()
 
        Const base64AESKey As String = "cWhay3H4asTvRzXzXGZQ3KyBEu9BZaIxl8J+L4Bhr5A="
        Const N As Integer = 3
        Const valSumCalc As String = "qx6p"
        '==============================================================
 
        Dim h As SHA256Managed
        Dim base64AESKeyBytes() As Byte
        Dim sha256hash() As Byte
 
        '' see https://msdn.microsoft.com/en-us/library/b388cb5s(v=vs.90).aspx Dimension Length
        '' The Index Of Each dimension Is 0-based, which means it ranges from 0 through 
        '' its upper bound. Therefore, the length of a given dimension Is greater by 1 than 
        '' the declared upper bound for that dimension.
        '' -> therefore array size = N-1
        Dim sha256hashNbytes(N - 1) As Byte
 
 
        Dim base64sha256hashNbytes As String
        Dim result As String
 
        h = New SHA256Managed()
        base64AESKeyBytes = System.Text.Encoding.ASCII.GetBytes(base64AESKey)
        sha256hash = h.ComputeHash(base64AESKeyBytes)
 
        Array.Copy(sha256hash, 0, sha256hashNbytes, 0, sha256hashNbytes.Length)
        base64sha256hashNbytes = Convert.ToBase64String(sha256hashNbytes)
        result = base64sha256hashNbytes.Replace("=", "")
 
        If (0 = String.Compare(result, valSumCalc)) Then
            Console.WriteLine("success")
        Else
            Console.WriteLine("error")
        End If
        Console.ReadLine()
    End Sub
 
End Module