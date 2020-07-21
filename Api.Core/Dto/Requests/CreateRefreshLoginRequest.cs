using Api.Core.Enums.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dto.Requests
{
    public class CreateRefreshLoginRequest
    {
        public string AuthToken { get; set; }
        public string Email { get; set; }
        public string AuthRefreshToken { get; set; }
        public EnumSignInType SignInType { get; set; }
    }
}
