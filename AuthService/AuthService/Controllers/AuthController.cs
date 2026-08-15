using AuthService.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost("register")]
        public IActionResult Register(RegisterDto request)
        {
            return Ok();
        }
       
    }
}
