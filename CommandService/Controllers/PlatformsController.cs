using Microsoft.AspNetCore.Mvc;
using System;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("---> Inbound Post At Commands Service");
            return Ok("Inbound Test Of Platform Controller At Commands Service");
        }
    }
}