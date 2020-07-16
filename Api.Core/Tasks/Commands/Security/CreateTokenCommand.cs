using Api.Core.Dto.Requests;
using Api.Core.Dto.Responses;
using Api.Core.Entities.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Tasks.Commands.Security
{
    public class CreateTokenCommand : IRequest<ResultTask<CreateLoginResponse>>
    {
        public User Request{ get; set; }
        public CreateTokenCommand(User request)
        {
            this.Request = request;
        }
    }
}
