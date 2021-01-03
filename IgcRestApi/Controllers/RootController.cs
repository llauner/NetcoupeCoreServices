using IgcRestApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IgcRestApi.Controllers
{
    [ApiController]
    [Route("")]
    public class RootController : ControllerBase
    {

        private readonly ILogger<RootController> _logger;

        public RootController(ILogger<RootController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        public PingResponse Get()
        {
            return new PingResponse("IgcRestApi");
        }
    }
}
