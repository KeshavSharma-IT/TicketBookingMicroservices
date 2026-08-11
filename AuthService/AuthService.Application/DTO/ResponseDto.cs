using AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.DTO
{
    public class ResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
