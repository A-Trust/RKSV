#include <stdio.h>
#include <stdlib.h>
#include <winscard.h>
#include <vector>
#pragma comment(lib, "winscard.lib")



const unsigned char ACOSID_ATR_V2_COLD[] = { 0x3B, 0xDF, 0x96, 0xFF, 0x91, 0x01, 0x31, 0xFE, 0x46, 0x80, 0x31, 0x90, 0x52, 0x41, 0x02, 0x64, 0x05, 0x02, 0x00, 0xAC, 0x73, 0xD6, 0x22, 0xC0, 0x17 };
const unsigned char ACOSID_ATR_V2_WARM[] = { 0x3B, 0xDF, 0x18, 0xFF, 0x91, 0x01, 0x31, 0xFE, 0x46, 0x80, 0x31, 0x90, 0x52, 0x41, 0x02, 0x64, 0x05, 0x02, 0x00, 0xAC, 0x73, 0xD6, 0x22, 0xC0, 0x99 };
const unsigned char ACOSID_ATR_V3[] = { 0x3B, 0xDF, 0x97, 0x00, 0x81, 0x31, 0xFE, 0x46, 0x80, 0x31, 0x90, 0x52, 0x41, 0x03, 0x64, 0x05, 0x02, 0x01, 0xAC, 0x73, 0xD6, 0x22, 0xC0, 0xF8 };
const unsigned char ACOSID_ATR_V41 =  { 0x3B, 0xDF, 0x97, 0x00, 0x81, 0x31, 0xFE, 0x46, 0x80, 0x31, 0x90, 0x52, 0x41, 0x04, 0x64, 0x05, 0x04, 0x01, 0xAC, 0x73, 0xD6, 0x22, 0xC0, 0xF9 };
// ATRs for earlier test cards
const unsigned char ACOSID_ATR_TEST0[] = { 0x3B,0xDF,0x97,0x00,0x81,0x31,0xFE,0x46,0x80,0x31,0x90,0x52,0x41,0x02,0x64,0x05,0xC9,0x03,0xAC,0x73,0xD6,0x22,0xC0,0x30 };
const unsigned char ACOSID_ATR_TEST1[] = { 0x3B,0xDF,0x18,0xFF,0x91,0x81,0x31,0xFE,0x46,0x80,0x31,0x90,0x52,0x41,0x02,0x64,0x05,0xC9,0x03,0xAC,0x73,0xD6,0x22,0xC0,0xD1 };


// Older card types
const unsigned char CARDOS_53_ATR[] = { 0x3b, 0xdf, 0x18, 0x00, 0x81, 0x31, 0xfe, 0x58, 0x80, 0x31, 0x90, 0x52, 0x41, 0x01, 0x64, 0x05, 0xc9, 0x03, 0xac, 0x73, 0xb7, 0xb1, 0xd4, 0x44 };
const unsigned char ACOS_04_ATR_1[] = { 0x3B, 0xBF, 0x13, 0x00, 0x81, 0x31, 0xFE, 0x45, 0x45, 0x50, 0x41 };
const unsigned char ACOS_04_ATR_2[] = { 0x3B, 0xBF, 0x11, 0x00, 0x81, 0x31, 0xFE, 0x45, 0x45, 0x50, 0x41 };



void ACOSID_PerformSignature(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard);
void ACOSID_ReadCertificate(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard);

void CARDOS_PerformSignature(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard);
void CARDOS_ReadCertificate(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard);
void ACOS_PerformSignature(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard);
void ACOS_ReadCertificate(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard);


bool CheckApduSW(BYTE *pbRecvBuffer, DWORD dwRecvLength, unsigned char sw1, unsigned char sw2);
bool ReadFile(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard, std::vector<unsigned char> &buf);

int main(void)
{
	LONG rv;
	SCARDCONTEXT hContext;
	char* mszReaders;
	SCARDHANDLE hCard;
	DWORD dwReaders, dwActiveProtocol;
	SCARD_IO_REQUEST pioSendPci;

	rv = SCardEstablishContext(SCARD_SCOPE_SYSTEM, NULL, NULL, &hContext);
	rv = SCardListReadersA(hContext, NULL, NULL, &dwReaders);
	mszReaders = new char[dwReaders * sizeof(char)];
	rv = SCardListReadersA(hContext, NULL, mszReaders, &dwReaders);
	printf("reader name: %s\n", mszReaders);

	rv = SCardConnectA(hContext, mszReaders, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1, &hCard, &dwActiveProtocol);

	switch (dwActiveProtocol)
	{
	case SCARD_PROTOCOL_T0:
		pioSendPci = *SCARD_PCI_T0;
		break;

	case SCARD_PROTOCOL_T1:
		pioSendPci = *SCARD_PCI_T1;
		break;
	}

	DWORD dwState, dwProtocol;
	BYTE atr[1024];
	DWORD atrsize = sizeof(atr);
	rv = SCardStatus(hCard, NULL, 0, &dwState, &dwProtocol, (LPBYTE)&atr, &atrsize);


	if (
	    ((sizeof(ACOSID_ATR_V41) <= atrsize) && (0 == memcmp(&atr, ACOSID_ATR_V41, sizeof(ACOSID_ATR_V41)))) ||
		((sizeof(ACOSID_ATR_V3) <= atrsize) && (0 == memcmp(&atr, ACOSID_ATR_V3, sizeof(ACOSID_ATR_V3)))) ||
		((sizeof(ACOSID_ATR_V2_WARM) <= atrsize) && (0 == memcmp(&atr, ACOSID_ATR_V2_WARM, sizeof(ACOSID_ATR_V2_WARM)))) ||
		((sizeof(ACOSID_ATR_V2_COLD) <= atrsize) && (0 == memcmp(&atr, ACOSID_ATR_V2_COLD, sizeof(ACOSID_ATR_V2_COLD)))) ||
		((sizeof(ACOSID_ATR_TEST0) <= atrsize) && (0 == memcmp(&atr, ACOSID_ATR_TEST0, sizeof(ACOSID_ATR_TEST0)))) ||
		((sizeof(ACOSID_ATR_TEST1) <= atrsize) && (0 == memcmp(&atr, ACOSID_ATR_TEST1, sizeof(ACOSID_ATR_TEST1)))) 
		)
	{
		printf("Card: ACOS-ID\n\n");

		printf("sign data\n");
		ACOSID_PerformSignature(pioSendPci, hCard);
		ACOSID_ReadCertificate(pioSendPci, hCard);
	}
	else if ((sizeof(CARDOS_53_ATR) <= atrsize) && (0 == memcmp(&atr, CARDOS_53_ATR, sizeof(CARDOS_53_ATR))))
	{
		printf("Card: CARDOS\n\n");

		printf("sign data\n");
		CARDOS_PerformSignature(pioSendPci, hCard);
		CARDOS_ReadCertificate(pioSendPci, hCard);
	}
	else if (
		((sizeof(ACOS_04_ATR_1) <= atrsize) && (0 == memcmp(&atr, ACOS_04_ATR_1, sizeof(ACOS_04_ATR_1)))) ||
		((sizeof(ACOS_04_ATR_2) <= atrsize) && (0 == memcmp(&atr, ACOS_04_ATR_2, sizeof(ACOS_04_ATR_2))))
		)
	{
		printf("Card: ACOS04\n\n");

		ACOS_PerformSignature(pioSendPci, hCard);
		ACOS_ReadCertificate(pioSendPci, hCard);
	}


	rv = SCardDisconnect(hCard, SCARD_LEAVE_CARD);
	delete[] mszReaders;
	rv = SCardReleaseContext(hContext);
	return 0;
}

void ACOSID_ReadCertificate(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard)
{
	BYTE pbRecvBuffer[258];
	DWORD dwRecvLength;
	LONG rv;
	unsigned char cmd1[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xDF, 0x01 };
	unsigned char cmd2[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xC0, 0x00 };

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd1, sizeof(cmd1), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd2, sizeof(cmd2), NULL, pbRecvBuffer, &dwRecvLength);

	std::vector<unsigned char> certificate;
	ReadFile(pioSendPci, hCard, certificate);

	//FILE *fp = fopen("c:\\temp\\cert.cer", "wb");
	//if (fp)
	//{
	//	fwrite(certificate.data(), 1, certificate.size(), fp);
	//	fclose(fp); 
	//}
}


void ACOSID_PerformSignature(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard)
{
	BYTE pbRecvBuffer[258];
	BYTE Hash[32 + 6];
	DWORD dwRecvLength;

	LONG rv;

	Hash[0] = 0x00;
	Hash[1] = 0x2A;
	Hash[2] = 0x9E;
	Hash[3] = 0x9A;
	Hash[4] = 32;

	for (int i = 0; i < 32; i++)
		Hash[5 + i] = i;
	Hash[37] = 0x00;


	dwRecvLength = sizeof(pbRecvBuffer);
	unsigned char cmd1[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xDF, 0x01 };
	unsigned char cmd2[] = { 0x00 , 0x20 , 0x00 , 0x8A , 0x08,  0x26, 0x12, 0x34, 0x56, 0xFF, 0xFF, 0xFF, 0xFF };

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd1, sizeof(cmd1), NULL, pbRecvBuffer, &dwRecvLength);

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd2, sizeof(cmd2), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&Hash, sizeof(Hash), NULL, pbRecvBuffer, &dwRecvLength);

	std::vector<unsigned char> sig;
	for (size_t i = 0; i < dwRecvLength - 2; i++)
		sig.push_back(pbRecvBuffer[i]);
}




void CARDOS_ReadCertificate(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard)
{
	BYTE pbRecvBuffer[258];
	DWORD dwRecvLength;
	LONG rv;
	unsigned char cmd1[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xDF, 0x01 };
	unsigned char cmd2[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xC0, 0x00 };

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd1, sizeof(cmd1), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd2, sizeof(cmd2), NULL, pbRecvBuffer, &dwRecvLength);

	std::vector<unsigned char> certificate;
	ReadFile(pioSendPci, hCard, certificate);

	//FILE *fp = fopen("c:\\temp\\cert.cer", "wb");
	//if (fp)
	//{
	//	fwrite(certificate.data(), 1, certificate.size(), fp);
	//	fclose(fp); 
	//}
}


void CARDOS_PerformSignature(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard)
{
	BYTE pbRecvBuffer[258];
	BYTE Hash[32 + 6];
	DWORD dwRecvLength;

	LONG rv;

	Hash[0] = 0x00;
	Hash[1] = 0x2A;
	Hash[2] = 0x9E;
	Hash[3] = 0x9A;
	Hash[4] = 32;

	for (int i = 0; i < 32; i++)
		Hash[5 + i] = i;
	Hash[37] = 0xFF;


	dwRecvLength = sizeof(pbRecvBuffer);
	unsigned char cmd1[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xDF, 0x01 };
	unsigned char cmd2[] = { 0x00 , 0x20 , 0x00 , 0x81 , 0x08,  0x26, 0x12, 0x34, 0x56, 0xFF, 0xFF, 0xFF, 0xFF };

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd1, sizeof(cmd1), NULL, pbRecvBuffer, &dwRecvLength);

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd2, sizeof(cmd2), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&Hash, sizeof(Hash), NULL, pbRecvBuffer, &dwRecvLength);

	std::vector<unsigned char> sig;
	for (size_t i = 0; i < dwRecvLength - 2; i++)
		sig.push_back(pbRecvBuffer[i]);
}

void ACOS_ReadCertificate(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard)
{
	BYTE pbRecvBuffer[258];
	DWORD dwRecvLength;
	LONG rv;
	unsigned char cmd1[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xDF, 0x70 };
	unsigned char cmd2[] = { 0x00 , 0xA4 , 0x00 , 0x0C, 0x02, 0xC0, 0x02 };

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd1, sizeof(cmd1), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd2, sizeof(cmd2), NULL, pbRecvBuffer, &dwRecvLength);

	std::vector<unsigned char> certificate;
	ReadFile(pioSendPci, hCard, certificate);

	//FILE *fp = fopen("c:\\temp\\cert.cer", "wb");
	//if (fp)
	//{
	//	fwrite(certificate.data(), 1, certificate.size(), fp);
	//	fclose(fp); 
	//}
}



void ACOS_PerformSignature(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard)
{
	BYTE pbRecvBuffer[258];
	BYTE Hash[32 + 5];
	DWORD dwRecvLength;

	LONG rv;

	Hash[0] = 0x00;
	Hash[1] = 0x2A;
	Hash[2] = 0x90;
	Hash[3] = 0x81;
	Hash[4] = 32;

	for (int i = 0; i < 32; i++)
		Hash[5 + i] = i;


	dwRecvLength = sizeof(pbRecvBuffer);
	unsigned char cmd1[] = { 0x00, 0xA4, 0x00, 0x0C, 0x02, 0xDF, 0x70 };
	unsigned char cmd2[] = { 0x00, 0x22, 0x41, 0xB6, 0x06, 0x84, 0x01, 0x88, 0x80, 0x01, 0x44 };
	unsigned char cmd3[] = { 0x00, 0x20, 0x00, 0x81, 0x08, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x00, 0x00 };
	unsigned char cmd4[] = { 0x00, 0x2A, 0x9E, 0x9A, 0x00 };

	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd1, sizeof(cmd1), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd2, sizeof(cmd2), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd3, sizeof(cmd3), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&Hash, sizeof(Hash), NULL, pbRecvBuffer, &dwRecvLength);
	dwRecvLength = sizeof(pbRecvBuffer);
	rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd4, sizeof(cmd4), NULL, pbRecvBuffer, &dwRecvLength);

	std::vector<unsigned char> sig;
	for (size_t i = 0; i < dwRecvLength - 2; i++)
		sig.push_back(pbRecvBuffer[i]);
}


bool CheckApduSW(BYTE *pbRecvBuffer, DWORD dwRecvLength, unsigned char sw1, unsigned char sw2)
{
	if (dwRecvLength < 2)
	{
		return false;
	}
	unsigned char c1 = pbRecvBuffer[dwRecvLength - 2];
	unsigned char c2 = pbRecvBuffer[dwRecvLength - 1];

	bool bRet = true;

	if (c1 != sw1)
	{
		bRet = false;
	}

	if (c2 != sw2)
	{
		bRet = false;
	}
	return bRet;
}


bool ReadFile(SCARD_IO_REQUEST pioSendPci, SCARDHANDLE hCard, std::vector<unsigned char> &buf)
{
	BYTE pbRecvBuffer[500];
	unsigned long fileIndex = 0;
	buf.clear();
	DWORD dwRecvLength;
	bool success = true;
	unsigned char cmd[] = { 0x00, 0xB0, 0x00, 0x00, 0xFF }; // read data
	unsigned long ulongmax = 0L - 1L;
	bool eof = false;


	while (!eof)
	{
		unsigned char b1 = (unsigned char)(fileIndex >> 8);
		unsigned char b2 = (unsigned char)fileIndex;
		unsigned char b3 = 0xFF;
		cmd[2] = b1;
		cmd[3] = b2;
		cmd[4] = b3;

		if (success)
		{
			dwRecvLength = sizeof(pbRecvBuffer);
			LONG rv = SCardTransmit(hCard, &pioSendPci, (unsigned char*)&cmd, sizeof(cmd), NULL, pbRecvBuffer, &dwRecvLength);
		}
		if (success)
		{
			success = CheckApduSW((BYTE*)&pbRecvBuffer, dwRecvLength, 0x90, 0x00);
		}

		if (!success)
		{
			if (CheckApduSW((BYTE*)&pbRecvBuffer, dwRecvLength, 0x62, 0x82))
			{
				// EOF
				success = true;
				eof = true;
			}
		}


		if (success)
		{
			unsigned long len = dwRecvLength;
			if (len > 2)
				len -= 2;

			for (size_t i = 0; i < len; ++i)
				buf.push_back(pbRecvBuffer[i]);

			if ((ulongmax - fileIndex) <= len)
			{
				break;
			}
			else
			{
				fileIndex += len;
			}
		}
		else
		{
			break;
		}
	}

	return success;
}