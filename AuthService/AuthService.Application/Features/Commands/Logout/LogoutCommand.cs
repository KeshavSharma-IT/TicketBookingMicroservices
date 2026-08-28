using MediatR;

namespace AuthService.Application.Features.Commands.Logout
{
    public class LogoutCommand : IRequest<bool>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
