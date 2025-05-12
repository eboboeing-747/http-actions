namespace http_request_monitoring_system
{
    public class ServerActions
    {
        public Dictionary<int, string> jsonData = new Dictionary<int, string>();
        public int GetAmount = 0;
        public int PostAmount = 0;

        public string AddData(string contents)
        {
            int id = this.jsonData.Count;
            this.jsonData.Add(id, contents);
            this.PostAmount++;

            return $"{{\"id\": {id}}}";
        }

        public string GetData(string idStr)
        {
            bool isValidId = int.TryParse(idStr, out int id);

            if (!isValidId)
                return "{errorMessage: failed to parse id}";
            
            return this.jsonData.ContainsKey(id) ? this.jsonData[id] : "{errorMessage: no item with such id}";
        }

        public string GetServerInfo(string body)
        {
            string response = $"{{\"requestAmount\": {this.GetAmount + this.PostAmount},\"uptime\": {Program.server.Uptime}}}";
            this.GetAmount++;

            return response;
        }
    }
}
