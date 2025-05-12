using http_request_monitoring_system.Objects;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;
using System.Runtime.CompilerServices;

namespace http_request_monitoring_system
{
    public delegate string Route(string body);

    public class HttpServer
    {
        public HttpListener listener = new HttpListener();
        public Thread listenerThread;
        public DateTime startTime;
        public Dictionary<string, Route> GetRouter { get; set; } = new Dictionary<string, Route>();
        public Dictionary<string, Route> PostRouter { get; set; } = new Dictionary<string, Route>();
        public Dictionary<string, Dictionary<string, Route>> Router { get; set; } = new Dictionary<string, Dictionary<string, Route>>();

        public HttpServer()
        {
            this.listenerThread = new Thread(HandleRequest);
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
                string? route = request.RawUrl;

                if (route == null)
                    responseString = $"failed to get route";
                else if (!Router[method].ContainsKey(route))
                    responseString = $"no \"{method}\" for \"{route}\"";
                else
                    responseString = this.Router[method][route](contents);

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
