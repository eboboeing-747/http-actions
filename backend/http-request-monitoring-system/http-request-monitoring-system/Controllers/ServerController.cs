using http_request_monitoring_system.Objects;
using Microsoft.AspNetCore.Mvc;

namespace http_request_monitoring_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        [HttpPost("start")]
        public ActionResult Start(
            [FromBody] int port
        ) {
            Program.server.Start(port);
            return Ok($"server is listening on port {port}");
        }

        [HttpGet("status")]
        public ActionResult Status()
        {
            return Ok(Program.server.listener.IsListening);
        }

        /*
        [HttpGet("shutdown")]
        public ActionResult Shutdown()
        {
            Program.server.Stop();
            return Ok();
        }
        */

        [HttpGet("test")]
        public ActionResult TestConcurrency()
        {
            return Ok($"thread is not blocked");
        }

        /*
        [HttpGet("stats")]
        public ActionResult Statistics()
        {

        }
        */
    }
}
