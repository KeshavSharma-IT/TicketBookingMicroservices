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
        //public IActionResult Register(RegisterDto user)
        //{
        //    if (user == null)
        //    {
        //        BadRequest("User is Empty");
        //    }

        //    if (user.Email != null)
        //    {
        //        UserDto user = _user.FindUserWithEmailAsync(user.Email);
        //        if (user)
        //        {
        //            BadRequest("This email is alreday register")
        //        }
        //    }

        //    UserDto user = _user.RegisterUser(user);

        //    return Ok(user);
        //}
    }
}
