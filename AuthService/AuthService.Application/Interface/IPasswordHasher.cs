using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interface
{
    public interface IPasswordHasher
    {
        //Task<string> PasswordHashAsync(string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
