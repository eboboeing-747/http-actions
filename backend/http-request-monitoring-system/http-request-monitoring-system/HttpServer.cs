using http_request_monitoring_system.Objects;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace http_request_monitoring_system
{
    public delegate string Route(string body);

    public class HttpServer
    {
        public int port;
        public HttpListener listener = new HttpListener();
        public Thread listenerThread;
        public Stopwatch uptime = new Stopwatch();
        public Dictionary<string, Dictionary<string, Route>> Router { get; set; } = new Dictionary<string, Dictionary<string, Route>>();

        public HttpServer()
        {
            this.listenerThread = new Thread(HandleRequest);
        }

        public void HandleRequest()
        {
            Stopwatch stopwatch = new Stopwatch();

            while (this.listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                stopwatch.Start();
                HttpListenerRequest request = context.Request;

                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                string contents = reader.ReadToEnd();
                string responseString = string.Empty;
                string method = request.HttpMethod;
                string? route = request.RawUrl;

                long unixTimestamp = (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
                Program.serverActions.serverInfo.uptime = this.uptime.ElapsedMilliseconds;

                if (route == null || !this.Router.ContainsKey(method))
                    responseString = $"failed to get route";
                else if (!Router[method].ContainsKey(route))
                    responseString = $"no \"{method}\" for \"{route}\"";
                else
                    responseString = this.Router[method][route](contents);

                HttpListenerResponse response = context.Response;
                // response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5500");
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Headers", "*");
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                if (this.Router.ContainsKey(method))
                {
                    long processingTime = stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();
                    RequestInfo requestInfo = new RequestInfo
                    {
                        processingTime = processingTime,
                        dateTime = unixTimestamp
                    };
                    Program.serverActions.serverInfo.Add(method, requestInfo);
                }
            }
        }

        public bool Start(int port)
        {
            if (this.listener.IsListening)
                return false;

            this.uptime.Start();
            this.listener.Prefixes.Add($"http://localhost:{port}/");
            this.listener.Start();
            this.listenerThread.Start();
            this.port = port;

            return true;
        }

        public void Stop()
        {
            this.uptime.Reset();
            this.listener.Stop();
        }

        public long Uptime
        {
            get
            {
                return this.uptime.ElapsedMilliseconds;
            }
        }
    }
}
