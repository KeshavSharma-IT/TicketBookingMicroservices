using AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }   
        public string PasswordHash { get; set; }  

        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
