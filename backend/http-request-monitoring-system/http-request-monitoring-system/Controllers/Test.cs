using http_request_monitoring_system.Objects;
using Microsoft.AspNetCore.Mvc;

namespace http_request_monitoring_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test : ControllerBase
    {
        [HttpPost("start")]
        public ActionResult Start(
            [FromBody] int port
        ) {
            Program.server.Start(port);
            return Ok($"server is listening on port {port}");
        }

        /*
        [HttpGet("info")]
        public ActionResult<InfoObject> Info()
        {
            return Ok(Program.server.Info);
        }
        */

        [HttpGet("test")]
        public ActionResult TestConcurrency()
        {
            return Ok($"thread is not blocked");
        }
    }
}
