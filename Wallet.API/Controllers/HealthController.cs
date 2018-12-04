using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Wallet.API.Controllers
{
    [Route("[controller]")]
    public class HealthController : BaseController
    {
        public HealthController(IConfiguration configuration)
            : base(configuration)
        { }

        /// <summary>
        /// Service health check.
        /// </summary>
        /// <returns>"pong" if the service is up and running.</returns>
        /// <response code="200">The service is up and running.</response>
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet("ping")]
        public ActionResult<string> Get()
        {
            return Ok("pong");
        }
    }
}
