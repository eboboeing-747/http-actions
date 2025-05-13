using System.Net;
using System.Runtime.CompilerServices;

namespace http_request_monitoring_system
{
    public class HttpClient
    {
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        async public Task<string> MakeRequest(string methodStr, string uri, string body)
        {
            // uri = "https://jsonplaceholder.typicode.com/posts";
            HttpMethod method = new HttpMethod(methodStr);

            HttpRequestMessage requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Content = new StringContent(body);
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.SendAsync(requestMessage);
            HttpStatusCode status = response.StatusCode;
            string responseBody = await response.Content.ReadAsStringAsync();

            return $"{(int)status} {status}\r\n\r\n{responseBody}";

            /*
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
            */
        }
    }
}
