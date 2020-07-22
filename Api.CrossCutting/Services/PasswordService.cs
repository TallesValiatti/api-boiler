using Api.Core.Interfaces.CrossCutting.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Api.CrossCutting.Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordService()
        {}
        public string CreatePassword(string password, string passwordHashkey)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                        password: password,
                        salt: Encoding.UTF8.GetBytes(passwordHashkey),
                        prf: KeyDerivationPrf.HMACSHA512,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public bool ValidatePassword(string password, string passwordHashkey, string passwordHash) => CreatePassword(password, passwordHashkey) == passwordHash;  
       
    }
}
