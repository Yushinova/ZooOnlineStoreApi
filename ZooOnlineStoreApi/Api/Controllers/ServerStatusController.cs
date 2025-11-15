using Microsoft.AspNetCore.Mvc;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ServerStatusController : ControllerBase
    {
        [HttpGet]
        public string GetStatus()
        {
            return"server is running";
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "pong";
        }
    }
}
