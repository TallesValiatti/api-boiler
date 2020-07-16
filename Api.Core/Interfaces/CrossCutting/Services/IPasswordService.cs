using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Interfaces.CrossCutting.Services
{
    public interface IPasswordService
    {
        string CreatePassword(string password, string passwordHashkey);
        bool ValidatePassword(string password, string passwordHashkey, string passwordHash);
    }
}
