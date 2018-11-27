using Microsoft.AspNetCore.Mvc;

namespace Wallet.API.Controllers
{
    [Route("[controller]")]
    public class HealthController : BaseController
    {
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
