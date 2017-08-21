using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace WebServer.Src
{
    public class HttpHelper
    {
        private class RequestData<T> where T : BaseRes
        {
            public string Method;
            public string Url;
            public Action<T> Callback;
        }

        private static void Get<T>(string url, Action<T> callback) where T : BaseRes
        {
            var data = new RequestData<T>() { Method = "GET", Url = url, Callback = callback };
            Send(data);
        }

        private static void Post<T>(string url, Action<T> callback) where T : BaseRes
        {
            var data = new RequestData<T>() { Method = "POST", Url = url, Callback = callback };
            Send(data);
        }

        private static void Send<T>(RequestData<T> data) where T : BaseRes
        {
            //ThreadPool.SetMaxThreads(1000, 1000);
            //ThreadPool.QueueUserWorkItem(new WaitCallback(RequestThread<T>), data);
            RequestThread<T>(data);
        }

        private static void RequestThread<T>(object obj) where T : BaseRes
        {
            var data = obj as RequestData<T>;

            try
            {
                Debug.Print(string.Format("Request:\nMethod: {0} URL: {1}", data.Method, data.Url));

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(data.Url);
                request.Method = data.Method;
                request.Timeout = 5000;
                request.ReadWriteTimeout = 5000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader reader = new StreamReader(responseStream);
                        var str = reader.ReadToEnd();
                        Debug.Print(string.Format("Response: {0}", str));

                        var res = JsonMapper.ToObject<T>(str);
                        /*
                        InvokeAsync(() =>
                        {
                        });
                        */
                        data.Callback(res);
                    }
                }
            }
            catch (WebException we)
            {
                Debug.Print("RequestError: " + we.Message);
            }
        }

        //=========================================Request List=====================================================
        public static void GetDataFromFlashAir(string url, Action<FlashRes> callback)
        {
            Get(url, callback);
        }
    }
}