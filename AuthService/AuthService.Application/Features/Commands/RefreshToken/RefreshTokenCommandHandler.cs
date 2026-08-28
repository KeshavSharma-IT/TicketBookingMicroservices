using AuthService.Application.DTO;
using AuthService.Application.Interface;
using AuthService.Application.IRepositiory;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository, 
            IJwtTokenGenerator jwtTokenGenerator, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<ResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // 1. Find user by Refresh Token
            var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken);

            if (user == null)
            {
                throw new Exception("Invalid refresh token.");
            }

            // 2. Validate token expiry
            if (user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Refresh token has expired. Please log in again.");
            }

            // 3. Generate new Access and Refresh tokens
            var newAccessToken = _jwtTokenGenerator.GenerateToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            // 4. Update the user entity in the DB
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Reset expiration
            
            await _userRepository.SaveChangesAsync();

            // 5. Map to response and return
            var response = _mapper.Map<ResponseDto>(user);
            response.Token = newAccessToken;
            response.RefreshToken = newRefreshToken;

            return response;
        }
    }
}
