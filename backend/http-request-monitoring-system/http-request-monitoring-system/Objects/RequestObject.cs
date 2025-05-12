namespace http_request_monitoring_system.Objects
{
    public class RequestObject
    {
        public string method { get; set; } = string.Empty;
        public string uri { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
    }
}