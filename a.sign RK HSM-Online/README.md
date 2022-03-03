# a.sign RK HSM/Online examples

## curl
```batch
curl.exe
  --cacert cas.pem 
  -o outputSign.txt 
  -X POST 
  -H "Content-Type: application/json"
  -d @requestSign.json 
  https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v1/u123456789/Sign
```

## C#
```csharp
using System.IO;
using System.Net;
 
namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            string URL = "https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v1/u123456789/Sign";
            string request = @"{
   ""password"":""123456789"",
   ""to_be_signed"":""c2FtcGxlIHRleHQgZm9yIHNpZ25pbmcgd2l0aCByZWdpc3RyaWVya2Fzc2UubW9iaWxlIG9ubGluZSBzZXJ2aWNl""
}";
            byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(request);
 
 
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URL);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
             
            webRequest.ContentLength = data.Length;
            webRequest.GetRequestStream().Write(data, 0, data.Length);
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
 
            StreamReader reader = new StreamReader(
                                          webResponse.GetResponseStream(), 
                                          System.Text.UTF8Encoding.UTF8);
            string ResponseText = reader.ReadToEnd();
        }
    }
}
```

## php
```php
<?php
 
print "Signatur";
// sign 
$url = 'https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v2/u123456789/Sign/JWS';
$data = '{"password":"123456789","jws_payload":"_R1-AT100_CASHBOX-DEMO-1_CASHBOX-DEMO-1-Receipt-ID-82_2016-03-11T04:24:46_0,00_0,00_0,00_0,00_0,00_NLoiSHL3bsM=_eee257579b03302f_cg8hNU5ihto="}';
 
$options = array(
    'http' => array(
        'header'  => "Content-type: application/json\r\n",
        'method'  => 'POST',
        'content' => $data,
    ),
);
 
$context  = stream_context_create($options);
$result = file_get_contents($url, false, $context);
var_dump($result);
 
 
print "\n\n\nZertifikatsdaten lesen";
// Get Certificate Data
$url2 = 'https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v2/u123456789/Certificate'; 
$options2 = array(
    'http' => array(
        'header'  => "Content-type: application/json\r\n",
        'method'  => 'GET',
    ),
);
$context2  = stream_context_create($options2);
$result2 = file_get_contents($url2, false, $context2);
var_dump($result2);
 
 
 
print "\n\n\nZDA ID lesen";
// Get ZDA
$url3 = 'https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v2/u123456789/ZDA'; 
$options3 = array(
    'http' => array(
        'header'  => "Content-type: application/json\r\n",
        'method'  => 'GET',
    ),
);
$context3  = stream_context_create($options3);
$result3 = file_get_contents($url3, false, $context3);
var_dump($result3);
?>
```