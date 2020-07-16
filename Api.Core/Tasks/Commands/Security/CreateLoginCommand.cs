using Api.Core.Dto.Requests;
using Api.Core.Dto.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Tasks.Commands.Security
{
    public class CreateLoginCommand : IRequest<ResultTask<CreateLoginResponse>>
    {
        public CreateLoginRequest Request{ get; set; }
        public CreateLoginCommand(CreateLoginRequest request)
        {
            this.Request = request;
        }
    }
}
