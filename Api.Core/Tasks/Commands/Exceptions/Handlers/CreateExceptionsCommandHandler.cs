using Api.Core.Dto.Responses;
using Api.Core.Entities.Security;
using Api.Core.Interfaces.CrossCutting.Services;
using Api.Core.Interfaces.Infra.Repositories.Security;
using Api.Core.Services.Microservices;
using Api.Core.Tasks.Commands.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Core.Tasks.Commands.Exceptions.Handlers
{
    class CreateExceptionsCommandHandler : IRequestHandler<CreateExceptionsCommand, ResultTask>
    {
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IFailureMicroservice _failureMicroservice;
        private readonly IUserRepository<User> _userRepository;

        public CreateExceptionsCommandHandler(
            IPasswordService passwordService,
            IUserRepository<User> userRepository,
            IMediator mediator,
            IFailureMicroservice failureMicroservice,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _passwordService = passwordService;
            _mediator = mediator;
            _failureMicroservice = failureMicroservice;
            _userRepository = userRepository;
        }
        public async Task<ResultTask> Handle(CreateExceptionsCommand command, CancellationToken cancellationToken)
        {
            try
            {
                //create the object to be returned on the end of this task
                var result = new ResultTask();

                //send msg to failure microservice
                await _failureMicroservice.SendMessageAsync(command.Exception);     
                
                return await Task.FromResult(result);
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }
    }
}
