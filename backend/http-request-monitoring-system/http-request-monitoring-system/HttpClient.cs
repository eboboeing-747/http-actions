using System.Runtime.CompilerServices;

namespace http_request_monitoring_system
{
    public class HttpClient
    {
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        async public Task<string> MakeRequest(string methodStr, string uri, string body)
        {
            // string server = "http://localhost:3000/register";

            // body = "{\"name\": \"name-0\", \"password\": \"password-0\"}";
            // uri = "https://jsonplaceholder.typicode.com/posts";
            HttpMethod method = new HttpMethod(methodStr);

            HttpRequestMessage requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Content = new StringContent(body);
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            try
            {
                using HttpResponseMessage response = await client.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);

                return e.Message;
            }
        }
    }
}
