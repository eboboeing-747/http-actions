using http_request_monitoring_system.Objects;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace http_request_monitoring_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        [HttpPost("send")]
        async public Task<IActionResult> Send(
            [FromBody] RequestObject request
        ) {
            // return Ok($"method:\n{request.method}\nuri:\n{request.uri}\nbody:\n{request.body}");
            string response = await Program.client.MakeRequest(request.method, request.uri, request.body);
            return Ok(response);
        }
    }
}
