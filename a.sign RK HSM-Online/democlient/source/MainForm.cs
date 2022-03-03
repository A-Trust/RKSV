using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DemoClient
{
    public partial class MainForm : Form
    {
        private const string LB = "\r\n";
        private const string KassenId = "demokasse42";
        private int umsatzzaehler = 0; 
        private const string Belegzeile = "_R1-AT1_1_1_2016-02-24T10:42:13_0,00_0,00_0,00_0,00_0,00_wBuMAAdZM4c=_987234644_qdXooXdnO+I=";
        private const string jws_heaer = "eyJhbGciOiJFUzI1NiJ9";
        private static string tosign;
        private string zdaid = "";
        private string certserialhex = "";
        private string sigcert = "";

        public MainForm()
        {
            InitializeComponent();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            tosign =  jws_heaer + "." + b64_url_encode(Encoding.UTF8.GetBytes(Belegzeile));
            label11.Text = "Kassen ID: " + KassenId;



            tbUser.Text = ConfigurationManager.AppSettings["username"].ToString();
            tbPwd.Text = ConfigurationManager.AppSettings["password"].ToString();

            tbpartneruser.Text = ConfigurationManager.AppSettings["partner_username"].ToString();
            tbpartnerpwd.Text = ConfigurationManager.AppSettings["partner_password"].ToString();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tbURL.SelectedIndex = 0;
            comboBox1.SelectedIndex = 1;
        }

        private string b64_url_encode(byte[] data)
        {
            string ret = Convert.ToBase64String(data);
            ret = ret.Replace("+", "-");
            ret = ret.Replace("/", "_");
            ret = ret.Replace("=", "");
            ret = ret.Replace("\r", "");
            ret = ret.Replace("\n", "");
            return ret; 
        }

        private byte[] b64_url_decode(string data)
        {
            data = data.Replace("-", "+");
            data = data.Replace("_", "/");

            while (data.Length % 4 != 0)
            {
                data += "=";
            }

            return Convert.FromBase64String(data);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadCert(); 

            // Sign
            string url = tbURL.Text + "/" + tbUser.Text + "/Sign";
            //string temp = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes("sdafgi9u04qghrq0ö94gtjgipg-sagjqwpoi4rtgjhwritgghiqerthihireqtgnzfv"));
            string temp = tosign;
            temp = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(temp));
            string exception = "";

            tbResult.Text = "Sign";
            string responseData;
            string requestData = "{\"password\":\"" + tbPwd.Text + "\", \"to_be_signed\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }


            // {"signature":"T2km8RiF7GxYW43YmLCZIEN72vcErMOekRP9A1UzSs46DqVUlHmuUOYmQZscv5JrgUOo6Xz9QrDbylkTMD_hyg"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["signature"];
            byte[] signature = b64_url_decode(sig);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // ZDA Info
            string url = tbURL.Text + "/" + tbUser.Text + "/ZDA";
            tbResult.Text = "Get ZDA Info";
            string responseData;
            string exception = "";

            if (ServerCommunication.Get(url, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadCert(); 
            // Sign Hash
            string url = tbURL.Text + "/" + tbUser.Text + "/Sign/Hash";

            byte[] bData = System.Text.ASCIIEncoding.ASCII.GetBytes(tosign);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bData);
            string temp = Convert.ToBase64String(hash);
            string exception = "";


            tbResult.Text = "Sign Hash";
            string responseData;
            string requestData = "{\"password\":\"" + tbPwd.Text + "\", \"hash\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }


            // {"signature":"i3OEisOVon-gHzVjYXCYlT67tTth0qPFt7y4Ttqc9JofwGa1n3EVUgln0MmAPtnBTgTxPDG5dHmrz5hncyaPSA"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["signature"];

            byte[] signature = b64_url_decode(sig);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);
        }



        private void LoadCert()
        {
            string txt = tb_zertifikat.Text;
            if(txt.Length <= 3)
            {
                button5_Click(null, null);
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            // Certificate
            string url = tbURL.Text + "/" + tbUser.Text + "/Certificate";
            tbResult.Text = "Get Certificate";
            string exception = "";

            string responseData;
            if (ServerCommunication.Get(url, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
                return;
            }

            // {"Signaturzertifikat":"MIIEvTCCA6WgAwIBAgIEOtgBVDANBgkqhkiG9w0BAQsFADCBoTELMAkGA1UEBgwCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEjMCEGA1UECwwaYS1zaWduLVByZW1pdW0tVGVzdC1TaWctMDIxIzAhBgNVBAMMGmEtc2lnbi1QcmVtaXVtLVRlc3QtU2lnLTAyMB4XDTE2MDIyNDE2NTIyMFoXDTIwMDQyNDE2NTIyMFowYzELMAkGA1UEBgwCQVQxGTAXBgNVBAMMEFVJRDogQVRVMTIzNDU2NzgxFDASBgNVBAQMC0FUVTEyMzQ1Njc4MQwwCgYDVQQqDANVSUQxFTATBgNVBAUMDDgxMzU2MzY4MzU0NTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABCu+KsFcy3SJ1+I\/taX\/+\/nWk7bintDPyCUIlr89A4oR7g+OEQe9\/4FoBw9wP5er83U\/95TH5\/0kyGfnZsPSEjajggIDMIIB\/zCBhAYIKwYBBQUHAQEEeDB2MEYGCCsGAQUFBzAChjpodHRwOi8vd3d3LmEtdHJ1c3QuYXQvY2VydHMvYS1zaWduLVByZW1pdW0tVGVzdC1TaWctMDIuY3J0MCwGCCsGAQUFBzABhiBodHRwOi8vb2NzcC10ZXN0LmEtdHJ1c3QuYXQvb2NzcDAOBgNVHQ8BAf8EBAMCBsAwJwYIKwYBBQUHAQMBAf8EGDAWMAgGBgQAjkYBATAKBggrBgEFBQcLATCBrgYDVR0fBIGmMIGjMIGgoIGdoIGahoGXbGRhcDovL2xkYXAtdGVzdC5hLXRydXN0LmF0L291PWEtc2lnbi1QcmVtaXVtLVRlc3QtU2lnLTAyIChTSEEtMjU2KSxvPUEtVHJ1c3QsYz1BVD9jZXJ0aWZpY2F0ZXJldm9jYXRpb25saXN0P2Jhc2U\/b2JqZWN0Y2xhc3M9ZWlkQ2VydGlmaWNhdGlvbkF1dGhvcml0eTAJBgNVHRMEAjAAMFkGA1UdIARSMFAwCAYGBACLMAEBMEQGBiooABEBCzA6MDgGCCsGAQUFBwIBFixodHRwOi8vd3d3LmEtdHJ1c3QuYXQvZG9jcy9jcC9hLXNpZ24tUHJlbWl1bTATBgNVHSMEDDAKgAhGBp+OQY4VvTARBgNVHQ4ECgQIQuZGSHtq57wwDQYJKoZIhvcNAQELBQADggEBANt70+A1GWNbyv5rNGTTCyr5YZ6tQFlvwLX+\/ALF+UPehNI0TzJv5rWMuZP+tV5nbg73QYIMoQBv+8Funj3LrQGuWRkgUlKbzJZQ2vTr5H4MvphvlQyM6QO3aZ1E11DaxbIOWLGkpuPCMA1mmk7msKmRpjtj18hqxAFGZzyhKtTBFviuDaSfo0eYHDgbGjPJundX8dKTQaabvqw5qZ0CRSIOjXYj++z+vEOXtqdKbPlwY41Y7t25cWNVWikMvhk2svHIHW3QJvHhdNWKhkLT51VfHN0DmWV1b1RekCbD8DdbcW9SZoCQvGgwnziBqNaOGsWPGdortMphw8X60qnfIGM=","Zertifizierungsstellen":["MIIEATCCAumgAwIBAgIEOWntwTANBgkqhkiG9w0BAQUFADCBlTELMAkGA1UEBhMCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEdMBsGA1UECwwUQS1UcnVzdC1UZXN0LVF1YWwtMDIxHTAbBgNVBAMMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMB4XDTE0MTEyNDE0NDkxN1oXDTI0MTExODEzNDkxN1owgaExCzAJBgNVBAYTAkFUMUgwRgYDVQQKDD9BLVRydXN0IEdlcy4gZi4gU2ljaGVyaGVpdHNzeXN0ZW1lIGltIGVsZWt0ci4gRGF0ZW52ZXJrZWhyIEdtYkgxIzAhBgNVBAsMGmEtc2lnbi1QcmVtaXVtLVRlc3QtU2lnLTAyMSMwIQYDVQQDDBphLXNpZ24tUHJlbWl1bS1UZXN0LVNpZy0wMjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANwJSfWpRaziThddTTup72CltlXl8oc7HQoK2SWsYQwZGAd5nJZbwbI4K8VFKlNnK72Zl8UhmQ2FxhzS6u+Q+qEzJOM2xTfA2NB6A9\/KFpTJXUjvCHgRvW16EEF9YpYXxKTSK+QrYCXAC5rL6SuYOzgA7Q1ivq+zLbyXxroux2zVEBIiaBGpZhOHGDFJk6h\/4QelIqzd2TIDCRzvhmLDVmhqX2C1NQb5kZuMgrxxOhG5Cr1F8solkwyu43JiM+apY4bZJVQBwi9ATBMz5tfdoLRslQy1BCQ4X+b6u\/2856gucU+1e\/wa5pB9Ff0eP+xy+j2DZOXLNd8m\/IQvnshjNusCAwEAAaNLMEkwDwYDVR0TAQH\/BAUwAwEB\/zARBgNVHQ4ECgQIRgafjkGOFb0wEwYDVR0jBAwwCoAIQg8xWXA9iecwDgYDVR0PAQH\/BAQDAgEGMA0GCSqGSIb3DQEBBQUAA4IBAQBq\/owq5eGvhxegchLvnMjPnE9gTYIHEvMq8DN6h2J7pTEhKG2o09LLn\/pNHWRjKENU\/LqZBIAJ5zebm5XqzB631BYcuu1abyPFfpMdAL9X4zFuDvg9EGaTir2c81XaBYzVSLN7fxmNLKSmMwUt0JQQyqpe3V9iyoBE\/WcQyKmKaEp7mCZsGFBm6KmJgqD6TPb7X9bWUr3yx6Z5gek71f3vQi69m1x811azXlxu1i\/XFnVpzxkrKRXJWC+wnQRxXmU7YnMzYPOA7UOpUG6J+7tYi29hY3EpMgyXM\/T\/BL5MdyzBefbPVzLHng5zVaXNpO0ENCrlUyi1m3Yd\/7QPDdJM","MIID4DCCAsigAwIBAgIEOWntvzANBgkqhkiG9w0BAQUFADCBlTELMAkGA1UEBhMCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEdMBsGA1UECwwUQS1UcnVzdC1UZXN0LVF1YWwtMDIxHTAbBgNVBAMMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMB4XDTE0MTEyNDE0NDc0N1oXDTI0MTExODEzNDc0N1owgZUxCzAJBgNVBAYTAkFUMUgwRgYDVQQKDD9BLVRydXN0IEdlcy4gZi4gU2ljaGVyaGVpdHNzeXN0ZW1lIGltIGVsZWt0ci4gRGF0ZW52ZXJrZWhyIEdtYkgxHTAbBgNVBAsMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMR0wGwYDVQQDDBRBLVRydXN0LVRlc3QtUXVhbC0wMjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANMBok2fNNtIEcf7Sw47vprkUeti6Y64Rc5rrAjh7cGwo4Jp5LyfvEVdv9AMNiuOX7ywd1xW99UZWtZ8MzXvWM5M6trLkeBYnCukwc9DqawXcuXXCYwgTuisFTmYO6GVJNr1iE\/LJdSKbu5AVDS3FwXixqyJkjv\/xWIwU4q86oATW8++8wb6Lu+fQlhBbn3Kqpavt6K+lwWSCb+8vIhB47IlKhJZwGqXfGV9l9dDgKYUbZiv3BBa+MRBUTvIcahEKz8hG2E8W4EgCwzISMpeStJtRHo\/tJnA90KfSBTcz0txrxpHwqFgKwJvgW6nIjY1Sv5MfY5YJiEWv0d7UUkvlScCAwEAAaM2MDQwDwYDVR0TAQH\/BAUwAwEB\/zARBgNVHQ4ECgQIQg8xWXA9iecwDgYDVR0PAQH\/BAQDAgEGMA0GCSqGSIb3DQEBBQUAA4IBAQApqSvkQyfbO2yDWewHwo1Zl32uGz41KMP5FYtA3BIcqh89paHwrW9KfcrybdUIneVz4iSnpyrDrS4LavfP8h\/Hl1kRmVZRUBsOJRvqc1fiC2B6IJRHrmayb\/DbXuyoOsk7Sr8M9xtAD3SzJCRkBrtjz\/U\/xQdU9TfV9SQyPN3qI+SR25\/LRZDhOKcIFJduVpTYzbnKTIkl3OUrHXVq5xddxX6XP8bUjT+SqGiDf15H6N5flNBsvolMSo0OoQXFiDuY33frQSrSbHbA2p\/MptwxA8JgGh4lrbgZZxjTvpO1wATBLDc3wGZkNuy+tNrrHAmE08B7fiExULHxzfaZEWSF"],"Zertifikatsseriennummer":"987234644","ZertifikatsseriennummerHex":"3AD80154","algo":"ES256"}

            var respObj = JObject.Parse(responseData);
            tb_zertifikat.Text = (string)respObj["Signaturzertifikat"];

            //LibBase64.Base64 b = new LibBase64.Base64(LibBase64.eBase64Type.jws_signature);
            //string cert_url = b.Encode(Convert.FromBase64String(tb_zertifikat.Text));
            //tbResult.Text += "\r\n Cert B64Url=" + cert_url;
            return;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            LoadCert();
            // Sign Plain Text
            string url = tbURL.Text + "/" + tbUser.Text + "/Sign/Plain";

            string temp = tosign;
            string exception = "";

            tbResult.Text = "Sign Plain Text";
            string responseData;
            tbResult.Text += "\r\ninput: " + temp;
            string requestData = "{\"password\":\"" + tbPwd.Text + "\", \"to_be_signed\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }


            // {"signature":"DBs8qo1IULVqiQZ_TK_fkeqgV7B3m-CCoqDlonuPAegFJGsc5V0IfabTP6LQUKuAqCvnP256mwJndQQX-O44AA"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["signature"];

            byte[] signature = b64_url_decode(sig);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);

            return;
        }

        private void VerifySignature(byte[] signature)
        {
            string temp = jws_heaer + "." + b64_url_encode(UTF8Encoding.UTF8.GetBytes(Belegzeile));
            X509CertificateParser parser = new X509CertificateParser();
            X509Certificate cert = parser.ReadCertificate(Convert.FromBase64String(tb_zertifikat.Text));


            int len = signature.Length / 2;

            BigInteger sssrVal = new BigInteger(signature, 0, len);
            BigInteger rVal = new BigInteger(1, signature, 0, len);

            BigInteger sssVal = new BigInteger(signature, len, len);
            BigInteger sVal = new BigInteger(1, signature, len, len);
            DerSequence seq = new DerSequence(new DerInteger(rVal), new DerInteger(sVal));
            signature = seq.GetDerEncoded();
            

            Org.BouncyCastle.Crypto.ICipherParameters pubkey = cert.GetPublicKey();
            ISigner signer = new DsaDigestSigner(new ECDsaSigner(), new Sha256Digest());
            signer.Init(false, pubkey);
            byte[] toBeSigned = UTF8Encoding.UTF8.GetBytes(temp);
            signer.BlockUpdate(toBeSigned, 0, toBeSigned.Length);
            bool ret = signer.VerifySignature(signature);

            tbResult.Text += "\r\nVerify Result=" + ret.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Change Password
            string url = tbURL.Text + "/" + tbUser.Text + "/Password";
            //string temp = "123456789";
            string temp = "g7i0yrhoiagsdfjkhl";
            string exception = "";

            tbResult.Text = "Change Password";
            string responseData;
            string requestData = "{\"currentpassword\":\"" + tbPwd.Text + "\", \"newpassword\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }

            tbResult.Text += "\r\nChange Password back";

            requestData = "{\"currentpassword\":\"" + temp + "\", \"newpassword\":\"" + tbPwd.Text + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // partner konto anlegen
            string url = tbURL.Text + "/" + tbpartneruser.Text + "/Account";
            string exception = "";

            tbResult.Text = "Create Partner Account";
            string responseData;
            string requestData = "{\"partner_password\":\"" + tbpartnerpwd.Text + "\", \"classification_key_type\":\"0\", \"classification_key\":\"ATU12345678\", \"email\":\"test@test.com\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            LoadCert();
            // Sign JWS
            string url = tbURL.Text + "/" + tbUser.Text + "/Sign/JWS";
            string temp = Belegzeile;
            string exception = "";

            tbResult.Text = "Sign JWS";
            string responseData;
            string requestData = "{\"password\":\"" + tbPwd.Text + "\", \"jws_payload\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
                return;
            }



            // {"result":"eyJhbGciOiJFUzI1NiJ9.X1IxLUFUMV8xXzFfMjAxNi0wMi0yNFQxMDo0MjoxM18wLDAwXzAsMDBfMCwwMF8wLDAwXzAsMDBfd0J1TUFBZFpNNGM9Xzk4NzIzNDY0NF9xZFhvb1hkbk8rST0.guBdYrLfPeznP_hUaD_PeUyzomk1hujG0fFMzb9MHIh2h6yO9EzCEhovftN9hVori6lVHOmrvJf5Ma7MIC-5mw"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["result"];


            string[] jws_parts =  sig.Split(new char[] { '.' });

            byte[] signature = b64_url_decode(jws_parts[2]);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);

        }


        private void button7_Click_1(object sender, EventArgs e)
        {
            // start session
            string url = tbURL.Text + "/Session/" + tbUser.Text;
            string exception = "";

            tbResult.Text = "Start Session";
            string responseData;
            string requestData = "{\"password\":\"" + tbPwd.Text + "\" }";
            if (ServerCommunication.Put(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }


            // {"sessionid":"gnuvrzkumakcgxmojfnhbynxahabzrps","sessionkey":"/+RnG2omT+5FQeJyBsq1yxE13Q5kh59yCHEGgMXa9wE="}
            var respObj = JObject.Parse(responseData);
            tbsessionid.Text = (string)respObj["sessionid"];
            tbsessionkey.Text = (string)respObj["sessionkey"];
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            // close session
            string url = tbURL.Text + "/Session/" + tbsessionid.Text;
            string exception = "";

            tbResult.Text = "Close Session";
            string responseData;
            if (ServerCommunication.Delete(url, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
                tbsessionid.Text = "";
                tbsessionkey.Text = "";
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            LoadCert();
            // Session sign jws
            string url = tbURL.Text + "/Session/" + tbsessionid.Text + "/Sign/JWS";
            string temp = Belegzeile;
            string exception = "";

            tbResult.Text = "Session Sign JWS";
            string responseData;
            string requestData = "{\"sessionkey\":\"" + tbsessionkey.Text + "\", \"jws_payload\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }


            // {"result":"eyJhbGciOiJFUzI1NiJ9.X1IxLUFUMV8xXzFfMjAxNi0wMi0yNFQxMDo0MjoxM18wLDAwXzAsMDBfMCwwMF8wLDAwXzAsMDBfd0J1TUFBZFpNNGM9Xzk4NzIzNDY0NF9xZFhvb1hkbk8rST0.b2u3bj8DjEqsl4fnS7fQIIdmvWNJd36c1ffn6SUuaUgyJ7hO_FOp18wBr9vJY9Kt-3x-I9XvSvrFdnaVA-IRQw"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["result"];

            string[] jws_parts = sig.Split(new char[] { '.' });

            byte[] signature = b64_url_decode(jws_parts[2]);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            LoadCert();
            // Session sign
            string url = tbURL.Text + "/Session/" + tbsessionid.Text + "/Sign";
            string temp = tosign;
            temp = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(temp));
            string exception = "";

            tbResult.Text = "Session Sign";
            string responseData;
            string requestData = "{\"sessionkey\":\"" + tbsessionkey.Text + "\", \"to_be_signed\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }

            // {"signature":"Wutmhf_ggKSolSARk_jSLeAfnsaXVit7W1TdR3s3ihkhc8qhssdEIJdH2PkfpdNRNirMzQjL5eZeEyvjd5-F_Q"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["signature"];
            byte[] signature = b64_url_decode(sig);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            LoadCert();
            // session sign hash
            string url = tbURL.Text + "/Session/" + tbsessionid.Text + "/Sign/Plain";
            string temp = tosign;
            string exception = "";

            tbResult.Text = "Session Sign Plain Text";
            string responseData;
            string requestData = "{\"sessionkey\":\"" + tbsessionkey.Text + "\", \"to_be_signed\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }


            // {"signature":"eHwsYviaeQ-DFvU-frDPcq8CWqmJMIqS8J0lHrXwOoRY-F_A08jhnGT7DRXnjT4kPAyP_GaqHU1cbxwUZN9JvQ"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["signature"];
            byte[] signature = b64_url_decode(sig);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            LoadCert();
            // session sign plain
            string url = tbURL.Text + "/Session/" + tbsessionid.Text + "/Sign/Hash";
            byte[] bData = System.Text.ASCIIEncoding.ASCII.GetBytes(tosign);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bData);
            string temp = Convert.ToBase64String(hash);
            string exception = "";

            tbResult.Text = "Session Sign Hash";
            string responseData;
            string requestData = "{\"sessionkey\":\"" + tbsessionkey.Text + "\", \"hash\":\"" + temp + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }

            // {"signature":"gX6XZlsZLtkATZzHoaEbFI7ivyZ-W6rkPUm1M9sSHbrOVBtne0tk__-sVeV5jqV0l6hoPhFvWVhn2B1_OvDKSA"}
            var respObj = JObject.Parse(responseData);
            string sig = (string)respObj["signature"];

            byte[] signature = b64_url_decode(sig);
            tbResult.Text += "\r\nSignature (b64): " + Convert.ToBase64String(signature);
            VerifySignature(signature);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            CheckInitData();
            CheckFirstLine();


            string aeskey = GetAes();
            int belegnum = GetBelegNum();

            decimal b_normal = MyParseDecimal(betrag_normal.Text);
            decimal b_ermaessigt_1 = MyParseDecimal(betrag_ermaessigt_1.Text);
            decimal b_ermaessigt_2 = MyParseDecimal(betrag_ermaessigt_2.Text);
            decimal b_null = MyParseDecimal(betrag_null.Text);
            decimal b_besonders = MyParseDecimal(betrag_besonders.Text);

            AddToUmsatzzaehler(b_normal);
            AddToUmsatzzaehler(b_ermaessigt_1);
            AddToUmsatzzaehler(b_ermaessigt_2);
            AddToUmsatzzaehler(b_null);
            AddToUmsatzzaehler(b_besonders);


            string dep = tbResult.Text;
            string[] lines = dep.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // first line = "DEP:"
            string last_dep_line = lines[lines.Length - 1];
            string sigVorigerBeleg = HashSigVorigerBeleg(last_dep_line);

            string belegzeile = BuildBelLine(belegnum, b_normal, b_ermaessigt_1, b_ermaessigt_2, b_null, b_besonders, sigVorigerBeleg);

            //sign
            string signed_dep = Signieren(belegzeile);

            dep += "\r\n" + signed_dep;
            tbResult.Text = dep; 

        }


        private string Signieren (string belegzeile)
        {
            string url = tbURL.Text + "/" + tbUser.Text + "/Sign/JWS";
            string exception = "";
            string responseData;
            string requestData = "{\"password\":\"" + tbPwd.Text + "\", \"jws_payload\":\"" + belegzeile + "\" }";
            if (!ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + exception;
                return "";
            }

            // {"result":"eyJhbGciOiJFUzI1NiJ9.X1IxLUFUMV9kZW1va2Fzc2U0Ml8xXzIwMjItMDEtMTdUMDk6MDg6MTZfMCwwMF8wLDAwXzAsMDBfMCwwMF8wLDAwX1liYjNZbXR3WVozenYxakk0RTlVMFE9PV80NkQzNDU4OV9wS3JrMUlEYmN4TT0.emu2A1rAGMUnAgTFGKGZT0ecCH_5x5zM1oi7ca3hLFBkUN2O2vmX_KBNknafT3vTRfQZ51O_1M9KLj6DIEurDQ"}
            var respObj = JObject.Parse(responseData);
            return (string)respObj["result"];
        }


        private void CheckInitData()
        {
            string dep = tbResult.Text;
            if (!dep.StartsWith("DEP:"))
            {
                umsatzzaehler = 0; 
                dep = "DEP:" + LB;
            }
            tbResult.Text = dep;


            if(zdaid.Length <= 0)
            {
                string url = tbURL.Text + "/" + tbUser.Text + "/ZDA";
                string responseData;
                string exception = "";
                ServerCommunication.Get(url, out responseData, out exception);
                // {"zdaid":"AT1"}
                var respObj = JObject.Parse(responseData);
                zdaid = (string)respObj["zdaid"];
            }

            if(certserialhex.Length <= 0)
            {
                string url = tbURL.Text + "/" + tbUser.Text + "/Certificate";
                string exception = "";
                string responseData;
                ServerCommunication.Get(url, out responseData, out exception);

                // {"Signaturzertifikat":"MIIEvTCCA6WgAwIBAgIEOtgBVDANBgkqhkiG9w0BAQsFADCBoTELMAkGA1UEBgwCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEjMCEGA1UECwwaYS1zaWduLVByZW1pdW0tVGVzdC1TaWctMDIxIzAhBgNVBAMMGmEtc2lnbi1QcmVtaXVtLVRlc3QtU2lnLTAyMB4XDTE2MDIyNDE2NTIyMFoXDTIwMDQyNDE2NTIyMFowYzELMAkGA1UEBgwCQVQxGTAXBgNVBAMMEFVJRDogQVRVMTIzNDU2NzgxFDASBgNVBAQMC0FUVTEyMzQ1Njc4MQwwCgYDVQQqDANVSUQxFTATBgNVBAUMDDgxMzU2MzY4MzU0NTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABCu+KsFcy3SJ1+I\/taX\/+\/nWk7bintDPyCUIlr89A4oR7g+OEQe9\/4FoBw9wP5er83U\/95TH5\/0kyGfnZsPSEjajggIDMIIB\/zCBhAYIKwYBBQUHAQEEeDB2MEYGCCsGAQUFBzAChjpodHRwOi8vd3d3LmEtdHJ1c3QuYXQvY2VydHMvYS1zaWduLVByZW1pdW0tVGVzdC1TaWctMDIuY3J0MCwGCCsGAQUFBzABhiBodHRwOi8vb2NzcC10ZXN0LmEtdHJ1c3QuYXQvb2NzcDAOBgNVHQ8BAf8EBAMCBsAwJwYIKwYBBQUHAQMBAf8EGDAWMAgGBgQAjkYBATAKBggrBgEFBQcLATCBrgYDVR0fBIGmMIGjMIGgoIGdoIGahoGXbGRhcDovL2xkYXAtdGVzdC5hLXRydXN0LmF0L291PWEtc2lnbi1QcmVtaXVtLVRlc3QtU2lnLTAyIChTSEEtMjU2KSxvPUEtVHJ1c3QsYz1BVD9jZXJ0aWZpY2F0ZXJldm9jYXRpb25saXN0P2Jhc2U\/b2JqZWN0Y2xhc3M9ZWlkQ2VydGlmaWNhdGlvbkF1dGhvcml0eTAJBgNVHRMEAjAAMFkGA1UdIARSMFAwCAYGBACLMAEBMEQGBiooABEBCzA6MDgGCCsGAQUFBwIBFixodHRwOi8vd3d3LmEtdHJ1c3QuYXQvZG9jcy9jcC9hLXNpZ24tUHJlbWl1bTATBgNVHSMEDDAKgAhGBp+OQY4VvTARBgNVHQ4ECgQIQuZGSHtq57wwDQYJKoZIhvcNAQELBQADggEBANt70+A1GWNbyv5rNGTTCyr5YZ6tQFlvwLX+\/ALF+UPehNI0TzJv5rWMuZP+tV5nbg73QYIMoQBv+8Funj3LrQGuWRkgUlKbzJZQ2vTr5H4MvphvlQyM6QO3aZ1E11DaxbIOWLGkpuPCMA1mmk7msKmRpjtj18hqxAFGZzyhKtTBFviuDaSfo0eYHDgbGjPJundX8dKTQaabvqw5qZ0CRSIOjXYj++z+vEOXtqdKbPlwY41Y7t25cWNVWikMvhk2svHIHW3QJvHhdNWKhkLT51VfHN0DmWV1b1RekCbD8DdbcW9SZoCQvGgwnziBqNaOGsWPGdortMphw8X60qnfIGM=","Zertifizierungsstellen":["MIIEATCCAumgAwIBAgIEOWntwTANBgkqhkiG9w0BAQUFADCBlTELMAkGA1UEBhMCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEdMBsGA1UECwwUQS1UcnVzdC1UZXN0LVF1YWwtMDIxHTAbBgNVBAMMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMB4XDTE0MTEyNDE0NDkxN1oXDTI0MTExODEzNDkxN1owgaExCzAJBgNVBAYTAkFUMUgwRgYDVQQKDD9BLVRydXN0IEdlcy4gZi4gU2ljaGVyaGVpdHNzeXN0ZW1lIGltIGVsZWt0ci4gRGF0ZW52ZXJrZWhyIEdtYkgxIzAhBgNVBAsMGmEtc2lnbi1QcmVtaXVtLVRlc3QtU2lnLTAyMSMwIQYDVQQDDBphLXNpZ24tUHJlbWl1bS1UZXN0LVNpZy0wMjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANwJSfWpRaziThddTTup72CltlXl8oc7HQoK2SWsYQwZGAd5nJZbwbI4K8VFKlNnK72Zl8UhmQ2FxhzS6u+Q+qEzJOM2xTfA2NB6A9\/KFpTJXUjvCHgRvW16EEF9YpYXxKTSK+QrYCXAC5rL6SuYOzgA7Q1ivq+zLbyXxroux2zVEBIiaBGpZhOHGDFJk6h\/4QelIqzd2TIDCRzvhmLDVmhqX2C1NQb5kZuMgrxxOhG5Cr1F8solkwyu43JiM+apY4bZJVQBwi9ATBMz5tfdoLRslQy1BCQ4X+b6u\/2856gucU+1e\/wa5pB9Ff0eP+xy+j2DZOXLNd8m\/IQvnshjNusCAwEAAaNLMEkwDwYDVR0TAQH\/BAUwAwEB\/zARBgNVHQ4ECgQIRgafjkGOFb0wEwYDVR0jBAwwCoAIQg8xWXA9iecwDgYDVR0PAQH\/BAQDAgEGMA0GCSqGSIb3DQEBBQUAA4IBAQBq\/owq5eGvhxegchLvnMjPnE9gTYIHEvMq8DN6h2J7pTEhKG2o09LLn\/pNHWRjKENU\/LqZBIAJ5zebm5XqzB631BYcuu1abyPFfpMdAL9X4zFuDvg9EGaTir2c81XaBYzVSLN7fxmNLKSmMwUt0JQQyqpe3V9iyoBE\/WcQyKmKaEp7mCZsGFBm6KmJgqD6TPb7X9bWUr3yx6Z5gek71f3vQi69m1x811azXlxu1i\/XFnVpzxkrKRXJWC+wnQRxXmU7YnMzYPOA7UOpUG6J+7tYi29hY3EpMgyXM\/T\/BL5MdyzBefbPVzLHng5zVaXNpO0ENCrlUyi1m3Yd\/7QPDdJM","MIID4DCCAsigAwIBAgIEOWntvzANBgkqhkiG9w0BAQUFADCBlTELMAkGA1UEBhMCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEdMBsGA1UECwwUQS1UcnVzdC1UZXN0LVF1YWwtMDIxHTAbBgNVBAMMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMB4XDTE0MTEyNDE0NDc0N1oXDTI0MTExODEzNDc0N1owgZUxCzAJBgNVBAYTAkFUMUgwRgYDVQQKDD9BLVRydXN0IEdlcy4gZi4gU2ljaGVyaGVpdHNzeXN0ZW1lIGltIGVsZWt0ci4gRGF0ZW52ZXJrZWhyIEdtYkgxHTAbBgNVBAsMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMR0wGwYDVQQDDBRBLVRydXN0LVRlc3QtUXVhbC0wMjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANMBok2fNNtIEcf7Sw47vprkUeti6Y64Rc5rrAjh7cGwo4Jp5LyfvEVdv9AMNiuOX7ywd1xW99UZWtZ8MzXvWM5M6trLkeBYnCukwc9DqawXcuXXCYwgTuisFTmYO6GVJNr1iE\/LJdSKbu5AVDS3FwXixqyJkjv\/xWIwU4q86oATW8++8wb6Lu+fQlhBbn3Kqpavt6K+lwWSCb+8vIhB47IlKhJZwGqXfGV9l9dDgKYUbZiv3BBa+MRBUTvIcahEKz8hG2E8W4EgCwzISMpeStJtRHo\/tJnA90KfSBTcz0txrxpHwqFgKwJvgW6nIjY1Sv5MfY5YJiEWv0d7UUkvlScCAwEAAaM2MDQwDwYDVR0TAQH\/BAUwAwEB\/zARBgNVHQ4ECgQIQg8xWXA9iecwDgYDVR0PAQH\/BAQDAgEGMA0GCSqGSIb3DQEBBQUAA4IBAQApqSvkQyfbO2yDWewHwo1Zl32uGz41KMP5FYtA3BIcqh89paHwrW9KfcrybdUIneVz4iSnpyrDrS4LavfP8h\/Hl1kRmVZRUBsOJRvqc1fiC2B6IJRHrmayb\/DbXuyoOsk7Sr8M9xtAD3SzJCRkBrtjz\/U\/xQdU9TfV9SQyPN3qI+SR25\/LRZDhOKcIFJduVpTYzbnKTIkl3OUrHXVq5xddxX6XP8bUjT+SqGiDf15H6N5flNBsvolMSo0OoQXFiDuY33frQSrSbHbA2p\/MptwxA8JgGh4lrbgZZxjTvpO1wATBLDc3wGZkNuy+tNrrHAmE08B7fiExULHxzfaZEWSF"],"Zertifikatsseriennummer":"987234644","ZertifikatsseriennummerHex":"3AD80154","algo":"ES256"}
                var respObj = JObject.Parse(responseData);
                certserialhex = (string)respObj["ZertifikatsseriennummerHex"];
                sigcert = (string)respObj["Signaturzertifikat"];
            }
        }


        private void CheckFirstLine()
        {
            string dep = tbResult.Text;
            string[] lines = dep.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if(lines.Length <= 1)
            {
                // startbeleg erzeugen
                int belegnum = GetBelegNum();
                string sigVorigerBeleg = HashSigVorigerBeleg(KassenId);
                string belegzeile = BuildBelLine(belegnum, 0, 0, 0, 0, 0, sigVorigerBeleg);
                string signed_dep = Signieren(belegzeile);
                dep += "\r\n" + signed_dep;
                tbResult.Text = dep;
            }

        }

        private string GetAes()
        {
            string aeskey = txtAes.Text;

            if(aeskey.Length <= 0)
            {
                byte[] key = Registrierkasse.RKAesIcm.GenerateKey();
                aeskey = Convert.ToBase64String(key);
                txtAes.Text = aeskey; 
            }
            return aeskey; 
        }

        private int GetBelegNum()
        {
            string stemp = txtBelegnum.Text;
            int num = 0; 

            if(stemp.Length <= 0)
            {
                num = 0; 
            }
            else
            {
                num = Int32.Parse(stemp);
            }
            num++;
            txtBelegnum.Text = num.ToString(); 
            return num; 
        }

        private void AddToUmsatzzaehler(decimal f)
        {
            //int cent = (int)(f * 100.0); // floating point errors?
            // -> use decimal
            decimal centF = (f * 100.0m);
            int cent = (int)centF;
            umsatzzaehler += cent; 
        }

        private string HashSigVorigerBeleg(string dep)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(dep);
            SHA256Managed s = new SHA256Managed();
            byte[] hash = s.ComputeHash(data);

            byte[] extracted = new byte[8];
            Array.Copy(hash, extracted, extracted.Length);

            return Convert.ToBase64String(extracted);
        }

        private string BuildBelLine(int belegnum, decimal b_normal, decimal b_ermaessigt_1, decimal b_ermaessigt_2, decimal b_null, decimal b_besonders, string sigVorigerBeleg)
        {
            string aeskey = GetAes();
            byte[] bAeskey = Convert.FromBase64String(aeskey);
            byte[] bEncrypted = Registrierkasse.RKAesIcm.Encrypt(umsatzzaehler, KassenId, belegnum.ToString(), bAeskey);
            string encrypted = Convert.ToBase64String(bEncrypted); 

            string bel = string.Format("_R1-{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}",
                zdaid,
                KassenId,
                belegnum.ToString(),
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                d2s(b_normal), d2s(b_ermaessigt_1), d2s(b_ermaessigt_2), d2s(b_null), d2s(b_besonders),
                encrypted,
                certserialhex,
                sigVorigerBeleg);
            return bel; 
        }


        private string d2s(decimal b)
        {
            string s = b.ToString("0.00").Replace(".", ",");
            return s; 
        }

        private decimal MyParseDecimal(string val)
        {
            var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;

            string val2 = val.Replace(",", currentCulture.NumberFormat.NumberDecimalSeparator);
            val2 = val.Replace(".", currentCulture.NumberFormat.NumberDecimalSeparator);
            return Decimal.Parse(val2, currentCulture);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JSON|*.json|all files|*.*";
            saveFileDialog1.Title = "save dep file";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("{");
                sw.WriteLine("  \"Belege-Gruppe\": [");
                sw.WriteLine("    {");
                sw.WriteLine("      \"Signaturzertifikat\":\"" + sigcert + "\",");
                sw.WriteLine("      \"Zertifizierungsstellen\": [\"\"],");
                sw.WriteLine("      \"Belege-kompakt\": [");

                string dep = tbResult.Text;
                string[] lines = dep.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for(int i=1; i < lines.Length; i++)
                {
                    string x = ",";

                    if (i == (lines.Length - 1))
                        x = ""; 
                    sw.WriteLine("        \"" + lines[i] + "\""+ x);
                }

                sw.WriteLine("      ]");
                sw.WriteLine("    }");
                sw.WriteLine("  ]");
                sw.WriteLine("}");
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string aeskey = GetAes();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JSON|*.json|all files|*.*";
            saveFileDialog1.Title = "save cmc file";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("{");
                sw.WriteLine("  \"base64AESKey\": \"" + aeskey + "\",");
                sw.WriteLine("  \"certificateOrPublicKeyMap\": {");
                sw.WriteLine("     \"" + certserialhex + "\": {");
                sw.WriteLine("     \"id\": \"" + certserialhex  + "\",");
                sw.WriteLine("     \"signatureDeviceType\": \"CERTIFICATE\",");
                sw.WriteLine("     \"signatureCertificateOrPublicKey\": \"" + sigcert + "\"");
                sw.WriteLine("     }");
                sw.WriteLine("  }");
                sw.WriteLine("}");
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // Certificate inkl. history
            string url = tbURL.Text + "/" + tbUser.Text + "/Certificates";
            tbResult.Text = "Get Certificate";
            string exception = "";

            string responseData;
            if (ServerCommunication.Get(url, out responseData, out exception))
            {
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
                return;
            }

            return;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // partner konto auflisten
            string url = tbURL.Text + "/" + tbpartneruser.Text + "/Account/List";
            string exception = "";

            tbResult.Text = "List Partner Account";
            string responseData;
            string requestData = "{\"partner_password\":\"" + tbpartnerpwd.Text + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                if (responseData.Length > 1000)
                {
                    // performance of winforms
                    responseData = responseData.Substring(0, 1000);
                }
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            // partner credits lesen
            string url = tbURL.Text + "/" + tbpartneruser.Text + "/Credits";
            string exception = "";

            tbResult.Text = "List Partner Account";
            string responseData;
            string requestData = "{\"partner_password\":\"" + tbpartnerpwd.Text + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                if (responseData.Length > 1000)
                {
                    // performance of winforms
                    responseData = responseData.Substring(0, 1000);
                }
                tbResult.Text += "\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\n" + exception;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            // ExtendService
            string url = tbURL.Text + "/" + tbpartneruser.Text + "/Service/Extend";
            string exception = "";

            tbResult.Text = "Extend Account";
            string responseData;
            string requestData = "{\"partner_password\":\"" + tbpartnerpwd.Text + "\", \"username\":\"" + textBox1.Text + "\" , \"product_version\":0 }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\nOK";
            }
            else
            {
                tbResult.Text += "\r\nFAILED " + exception;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            // new certificate for extended service
            string url = tbURL.Text + "/" + tbpartneruser.Text + "/Certificate/New";
            string exception = "";

            tbResult.Text = "Neues Zertifikate für bestehenden Account";
            string responseData;
            string requestData = "{\"partner_password\":\"" + tbpartnerpwd.Text + "\", \"username\":\"" + textBox1.Text + "\" }";
            if (ServerCommunication.Post(url, requestData, out responseData, out exception))
            {
                tbResult.Text += "\r\nOK\r\n" + responseData;
            }
            else
            {
                tbResult.Text += "\r\nFAILED " + exception;
            }
        }

    }
}
