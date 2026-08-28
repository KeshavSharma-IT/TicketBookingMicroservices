using AuthService.Application.DTO;
using AuthService.Application.Features.Commands.Login;
using AuthService.Application.Features.Commands.RefreshToken;
using AuthService.Application.Features.Commands.Register;
using AuthService.Application.Features.Commands.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var command = new RegisterUserCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var command = new LoginUserCommand 
            { 
                Email = login.Email, 
                Password = login.Password 
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest("Invalid token or user already logged out.");
            }
            return Ok("Logged out successfully.");
        }
    }
}
