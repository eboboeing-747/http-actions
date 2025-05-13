using http_request_monitoring_system.Objects;
using System.Text.Json;

namespace http_request_monitoring_system
{
    public class ServerActions
    {
        public Dictionary<int, string> jsonData = new Dictionary<int, string>();
        public InfoObject serverInfo = new InfoObject();

        public string AddData(string contents)
        {
            int id = this.jsonData.Count;
            this.jsonData.Add(id, contents);

            return $"{{\"id\": {id}}}";
        }

        public string GetData(string idStr)
        {
            bool isValidId = int.TryParse(idStr, out int id);

            if (!isValidId)
                return "{errorMessage: failed to parse id}";
            
            this.jsonData.TryGetValue(id, out string? data);
            return data ?? "{errorMessage: no item with such id}"; // (return data != null ? data : errorMessage)
        }

#pragma warning disable IDE0060
        public string GetServerInfo(string body)
        {
            string response = JsonSerializer.Serialize(this.serverInfo);
            // string response = $"{{\"requestInfo\": {{\"get\": {{\"amount\": }} \"{this.GetAmount}\", \"post\": {this.PostAmount}}},\"uptime\": {Program.server.Uptime}}}";

            return response;
        }
#pragma warning restore IDE0060
    }
}
