using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Linq;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        [Authorize(Roles = "Admin",Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult GetStock()
        {

            var userName = HttpContext.User.Identity.Name;
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userMail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            return Ok($"StockValues=> Username:{userName} Id:{userId.Value} Mail:{userMail.Value}");
        }

    }
}
