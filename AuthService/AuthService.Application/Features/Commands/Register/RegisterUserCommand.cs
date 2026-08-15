using AuthService.Application.DTO;
using MediatR;

namespace AuthService.Application.Features.Commands.Register
{
    public record RegisterUserCommand(RegisterDto Register) : IRequest<ResponseDto>;
}

