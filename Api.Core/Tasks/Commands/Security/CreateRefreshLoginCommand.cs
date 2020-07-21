using Api.Core.Dto.Requests;
using Api.Core.Dto.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Tasks.Commands.Security
{
    public class CreateRefreshLoginCommand : IRequest<ResultTask<CreateLoginResponse>>
    {
        public CreateRefreshLoginRequest Request{ get; set; }
        public CreateRefreshLoginCommand(CreateRefreshLoginRequest request)
        {
            this.Request = request;
        }
    }
}
