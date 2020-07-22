using Api.Core.Enums.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dto.Requests
{
    public class CreateLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public EnumSignInType SignInType { get; set; }
    }
}
