
using AuthService.Domain.Entities;

namespace AuthService.Application.IRepositiory
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string Email);
        Task AddAsync(User user);

        Task SaveChangesAsync();
    }
}
