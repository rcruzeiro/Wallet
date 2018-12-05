using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Wallet.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;

        protected BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
