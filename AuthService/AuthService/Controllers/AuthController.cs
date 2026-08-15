
using AuthService.Application.DTO;
using AuthService.Application.Features.Commands.Register;
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
    }
}

