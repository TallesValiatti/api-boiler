using Api.Core.Enums.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Entities.Security
{
    public class SignInRegister : BaseEntity
    {
        public Guid UserId { get; set; }
        public string AuthToken { get; set; }
        public string AuthRefreshToken { get; set; }
        public EnumSignInType SignInType { get; set; }
        public DateTime AuthRefreshTokenValidation { get; set; }
        public User User { get; set; }
    }
}
