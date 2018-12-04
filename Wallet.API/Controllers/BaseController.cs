using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Wallet.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;
        protected readonly string _writeConnection =
            Environment.GetEnvironmentVariable("DB_CONNECTION_DEFAULT_NAME");
        protected readonly string _readConnection =
            Environment.GetEnvironmentVariable("DB_CONNECTION_READ_NAME");
        protected readonly string _mongoConnection;


        protected BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            _mongoConnection =
                Environment.GetEnvironmentVariable("MONGO_CONNECTION");
        }
    }
}
