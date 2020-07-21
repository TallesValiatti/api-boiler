using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Dto.Responses
{
    public class CreateLoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
