using AuthService.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Features.Commands.Login
{
    public class LoginUserCommand :IRequest<ResponseDto>
    {

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

       
    }
}
