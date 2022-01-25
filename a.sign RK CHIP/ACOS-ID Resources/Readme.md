## Ressourcen für die neue ACOS-ID Smartcard.
Im Jahr 2022 werden bestehende RKSV-Chipkarten (Typ CardOS 5.3 und ACOS 4) durch eine neue Kartengeneration (ACOS-ID) ersetzt. Auch die APDUs (bei direkter Verwendung der APDUs) und/oder die PKCS#11 DLL müssen für die neue Karte aktualisiert werden. 

Dieses Verzeichnis enthält:

- **ActivateAcosID** ist ein Aktivierungsprogramm für ACOS-ID Testkarten. Einfach in einen Ordner entpacken und ausführen - keine Abhängigkeiten. Achtung dieses Programm ist nur für Testkarten geeignet.
- **Test ASignClient DLL** enthält 32- und 64-Bit Versionen der PKCS#11 DLL, sowohl auch das ASignCards Programm, angepasst für die ACOS-ID Karte, welche genutzt werden können, um die aktivierte ACOS-ID Karte auszulesen/prüfen. Einfach die beiden Dateien in einen Ordner kopieren und AsignCards starten.
