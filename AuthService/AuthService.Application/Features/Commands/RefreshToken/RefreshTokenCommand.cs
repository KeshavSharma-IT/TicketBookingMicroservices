using AuthService.Application.DTO;
using MediatR;

namespace AuthService.Application.Features.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ResponseDto>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
