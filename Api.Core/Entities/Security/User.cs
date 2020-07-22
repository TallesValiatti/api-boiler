using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Entities.Security
{
    public class User : BaseEntity
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String PasswordHash { get; set; }
        public IList<SignInRegister> SignInRegisters { get; set; }
    }
}
