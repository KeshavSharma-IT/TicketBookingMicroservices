using AuthService.Application.DTO;
using AuthService.Application.Interface;
using AuthService.Application.IRepositiory;
using AuthService.Domain.Entities;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Features.Commands.Login
{
    public class LoginUserCommandHandler :IRequestHandler<LoginUserCommand,ResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;


        public LoginUserCommandHandler(IUserRepository userRepository,IPasswordHasher passwordHasher,IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ResponseDto> Handle(LoginUserCommand command,CancellationToken cancellationToken)
        {
            if(!string.IsNullOrWhiteSpace(command.Email) && !string.IsNullOrWhiteSpace(command.Password))
            {
                User user= await _userRepository.GetByEmailAsync(command.Email);
                if (user==null)
                {
                    throw new Exception("User Not found");
                }
                bool passmatch=  _passwordHasher.VerifyPassword(command.Password, user.PasswordHash);
                if (passmatch)
                {
                    var token = _jwtTokenGenerator.GenerateToken(user);
                    var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

                    user.RefreshToken= refreshToken;
                    user.RefreshTokenExpiryTime=DateTime.UtcNow.AddDays(1);

                    await _userRepository.SaveChangesAsync();


                    var response= _mapper.Map<ResponseDto>(user);
                    response.Token = token;
                    response.RefreshToken = refreshToken;   

                    return response;
                }
               
            }
             throw new Exception("User email and pass is not correct");
        }
    }
}
