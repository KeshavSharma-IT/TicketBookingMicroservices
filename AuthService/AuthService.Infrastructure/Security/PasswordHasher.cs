using AuthService.Application.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher =new();

        public string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException("Password can't be Empty");
           return _passwordHasher.HashPassword("", password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("Password can't be Empty");
            if (string.IsNullOrWhiteSpace(passwordHash)) return false;

              var result=_passwordHasher.VerifyHashedPassword("", passwordHash, password);

            return result == PasswordVerificationResult.Success ||
                    result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
