using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Api.Core.Interfaces.CrossCutting.Services
{
    public interface IAuthTokenService
    {
        string GenerateToken(Claim[] claims, string key, int time);
        string GenerateRefresToken(string key);
    }
}
