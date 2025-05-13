using System.Diagnostics;
using System.Text.Json.Serialization;

namespace http_request_monitoring_system.Objects
{
    public class RequestInfo
    {
        public long processingTime { get; set; } = 0;
        public long dateTime { get; set; } = 0;
    }

    public class MethodInfo
    {
        public int amount { get; set; } = 0;
        public long overallTime { get; set; } = 0;
        public List<RequestInfo> list { get; set; } = new List<RequestInfo>();

        public void Add(RequestInfo requestInfo)
        {
            this.amount++;
            this.overallTime += requestInfo.processingTime;
            this.list.Add(requestInfo);
        }
    }

    public class InfoObject
    {
        public long uptime { get; set; } = 0;
        public Dictionary<string, MethodInfo> graphInfo { get; set; } = new Dictionary<string, MethodInfo>();

        public InfoObject()
        {
            this.graphInfo.Add("GET", new MethodInfo());
            this.graphInfo.Add("POST", new MethodInfo());
        }

        public void Add(string method, RequestInfo methodInfo)
        {
            this.graphInfo[method].Add(methodInfo);
        }
    }
}
