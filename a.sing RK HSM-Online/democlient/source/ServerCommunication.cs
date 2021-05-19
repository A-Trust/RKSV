using System;
using System.IO;
using System.Net;


namespace DemoClient
{
    class ServerCommunication
    {

        public static bool Delete(string fullurl, out string ResponseData, out string exception)
        {
            return Get_Delete(fullurl, "DELETE", out ResponseData, out exception);
        }

        public static bool Get(string fullurl, out string ResponseData, out string exception)
        {
            return Get_Delete(fullurl, "GET",out ResponseData, out exception);
        }


        public static bool Get_Delete(string fullurl, string Method, out string ResponseData, out string exception)
        {
            ResponseData = "";
            exception = "";
            try
            {
                Uri url = new Uri(fullurl);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = Method;
                webRequest.ContentType = "application/json";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), System.Text.UTF8Encoding.UTF8);
                ResponseData = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                exception = "GET EXCEPTION: !!!! \n\n" + ex.Message;
                return false;
            }
            return true;
        }



        public static bool Post(string fullurl, string RequestData, out string ResponseData, out string exception)
        {
            return Post_Put(fullurl, "POST", RequestData, out ResponseData, out exception);
        }

        public static bool Put(string fullurl, string RequestData, out string ResponseData, out string exception)
        {
            return Post_Put(fullurl, "PUT", RequestData, out ResponseData, out exception);
        }


        private static bool Post_Put(string fullurl, string Method, string RequestData, out string ResponseData, out string exception)
        {
            ResponseData = "";
            exception = "";
            try
            {
                Uri url = new Uri(fullurl);

                byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(RequestData);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = Method;
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = data.Length;
                webRequest.GetRequestStream().Write(data, 0, data.Length);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), System.Text.UTF8Encoding.UTF8);
                ResponseData = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                exception = "POST EXCEPTION: !!!! \n\n" + ex.Message;
                return false;
            }

            return true;

        }
    }
}
