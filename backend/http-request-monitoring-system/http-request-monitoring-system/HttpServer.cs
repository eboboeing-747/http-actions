using http_request_monitoring_system.Objects;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;
using System.Runtime.CompilerServices;

namespace http_request_monitoring_system
{
    public class HttpServer
    {
        public int GetAmount = 0;
        public int PostAmount = 0;
        public HttpListener listener = new HttpListener();
        public Thread listenerThread;
        public Dictionary<int, string> jsonData = new Dictionary<int, string>();
        public DateTime startTime;

        public HttpServer()
        {
            this.listenerThread = new Thread(HandleRequest);
        }

        public string HandleGet()
        {
            string response = $"{{\"requestAmount\": {this.GetAmount + this.PostAmount},\"uptime\": {this.Uptime}}}";
            this.GetAmount++;

            return response;
        }

        public string HandlePost(string contents, out int id)
        {
            id = this.jsonData.Count;
            this.jsonData.Add(id, contents);
            this.PostAmount++;

            return $"{{\"id\": \"{id}\"}}";
        }

        public void HandleRequest()
        {
            while (this.listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                string contents = reader.ReadToEnd();
                string responseString = string.Empty;
                string method = request.HttpMethod;

                if (method == "GET")
                    responseString = this.HandleGet();
                else if (method == "POST")
                    responseString = this.HandlePost(contents, out int id);
                else
                    return; // tmp

                HttpListenerResponse response = context.Response;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        /*
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;

            Console.WriteLine($"--METHOD--\n{request.HttpMethod}");

            Console.WriteLine("--HEADERS--");
            NameValueCollection headers = request.Headers;
            for (int i = 0; i < headers.Count; i++)
            {
                Console.WriteLine($"{headers.GetKey(i)}: {headers.Get(i)}");
            }

            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            string contents = reader.ReadToEnd();
            Console.WriteLine($"--BODY--\n{contents}");
        */

        public void Start(int port)
        {
            this.startTime = DateTime.UtcNow;
            this.listener.Prefixes.Add($"http://localhost:{port}/");
            this.listener.Start();
            this.listenerThread.Start();
        }

        public long Uptime
        {
            get
            {
                // long unixTimestamp = (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds;

                DateTime now = DateTime.UtcNow;
                long uptime = (long)now.Subtract(this.startTime).TotalMilliseconds;
                return uptime;
            }
        }
    }
}
