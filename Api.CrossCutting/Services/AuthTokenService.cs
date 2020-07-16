using Api.Core.Interfaces.CrossCutting.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.CrossCutting.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        public string GenerateToken(Claim[] claims, string key, int time)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var byteKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(time),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(byteKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
