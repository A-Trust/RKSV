Imports Registrierkasse

Module Module1

    Sub Test1()
        Console.WriteLine("TEST1 - A-SIT Test Daten Entschlüsseln")

        Dim kassenID As String
        Dim belegnummer As String
        kassenID = "DEMO-CASH-BOX426"
        belegnummer = "776732"
        Console.WriteLine("Kassen ID: " + kassenID)
        Console.WriteLine("Belegnummer: " + belegnummer)

        Dim aesB64 As String
        aesB64 = "K7L9NyWiC1xKuyBO/7DuoRFqsqgq+m3t8hjOCMNXBio="

        Dim aesBytes As Byte()
        aesBytes = Convert.FromBase64String(aesB64)

        Dim encB64 As String
        encB64 = "4VjrD6b2vGo="

        Dim encBytes As Byte()
        encBytes = Convert.FromBase64String(encB64)

        Dim umsatz As Long
        umsatz = RKAesIcm.Decrypt(encBytes, kassenID, belegnummer, aesBytes)

        Console.WriteLine("Entschlüsselter Umsatz : " + umsatz.ToString())
        Console.WriteLine("")
    End Sub

    Sub Test2()
        Console.WriteLine("TEST2 - Verschlüsseln/Entschlüsseln")

        REM Generate new AES Key
        Dim aesKey As Byte()
        aesKey = RKAesIcm.GenerateKey()
        Console.WriteLine("AES key (B64): " + Convert.ToBase64String(aesKey))

        Dim kassenID As String
        Dim belegnummer As String
        kassenID = "Register3874"
        belegnummer = "39920034"
        Console.WriteLine("Kassen ID: " + kassenID)
        Console.WriteLine("Belegnummer: " + belegnummer)

        Dim umsatz As Long
        umsatz = 2349 ' Euro-Cent
        Console.WriteLine("Umsatz: " + umsatz.ToString())


        Dim encryptedBytes As Byte()
        encryptedBytes = RKAesIcm.Encrypt(umsatz, kassenID, belegnummer, aesKey)
        Console.WriteLine("Verschlüsselter Umsatz (B64): " + Convert.ToBase64String(encryptedBytes))

        Dim umsatz2 As Long
        umsatz2 = RKAesIcm.Decrypt(encryptedBytes, kassenID, belegnummer, aesKey)
        Console.WriteLine("Entschlüsselter Umsatz : " + umsatz2.ToString())
        Console.WriteLine("")
    End Sub



    Sub Main()
        REM TEST 1 - A-SIT Test Daten Entschlüsseln
        Test1()

        REM TEST2 - Verschlüsseln/Entschlüsseln
        Test2()

        Console.ReadLine()
    End Sub

End Module
