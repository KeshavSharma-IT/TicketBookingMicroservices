using AuthService.Application.DTO;
using AuthService.Application.Interface;
using AuthService.Application.IRepositiory;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<ResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existing = await _userRepository.GetByEmailAsync(request.Register.Email);

            if (existing != null)
            {
                throw new Exception("Email is already registered.");
            }

            // Hash password
            var hashedPassword = _passwordHasher.HashPassword(request.Register.Password);

            // Create new User entity
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Register.Name,
                Email = request.Register.Email,
                PasswordHash = hashedPassword,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            // Save user via Repository
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Return response DTO
            return new ResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
