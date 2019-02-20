using Microsoft.AspNetCore.Mvc;
using NLog;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Api.Extensions;

namespace Api.Controllers
{
    [ApiExceptionFilter]
    public abstract class BaseController : Controller
    {
        protected static Logger Logger = LogManager.GetCurrentClassLogger();
        protected BaseController()
        {

        }
        public string ClientId => this.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid).Value;
    }
}
