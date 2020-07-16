using Api.Core.Dto.Requests;
using Api.Core.Dto.Responses;
using Api.Core.Entities.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Tasks.Commands.Exceptions
{
    public class CreateExceptionsCommand : IRequest<ResultTask>
    {
        public Exception Exception { get; set; }
        public CreateExceptionsCommand(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
