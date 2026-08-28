using AuthService.Application.IRepositiory;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public LogoutCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // 1. Find user by their active Refresh Token
            var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken);

            if (user == null)
            {
                return false; // Token already revoked or invalid
            }

            // 2. Clear token data (Revocation)
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            // 3. Persist changes
            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}
