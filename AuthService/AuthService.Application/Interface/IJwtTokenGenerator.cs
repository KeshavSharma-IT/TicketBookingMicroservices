
using AuthService.Domain.Entities;

namespace AuthService.Application.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
