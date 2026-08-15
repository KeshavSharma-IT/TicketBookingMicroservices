using AuthService.Application.DTO;
using MediatR;


namespace AuthService.Application.Commands.Register
{
    public class RegisterCommand  :IRequest<ResponseDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
