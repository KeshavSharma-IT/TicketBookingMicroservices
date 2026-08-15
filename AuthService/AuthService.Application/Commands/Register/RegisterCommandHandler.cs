using AuthService.Application.DTO;
using AuthService.Application.IRepositiory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand,ResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<ResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Email != null) {
            
            }

            throw new NotImplementedException();
        }
    }
}
