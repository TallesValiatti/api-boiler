using Api.Core.Dto.Responses;
using Api.Core.Entities.Security;
using Api.Core.Enums.Security;
using Api.Core.Interfaces.CrossCutting.Services;
using Api.Core.Interfaces.Infra.Repositories.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Core.Tasks.Commands.Security.Handlers
{
    class CreateRefreshLoginCommandHandler : IRequestHandler<CreateRefreshLoginCommand, ResultTask<CreateLoginResponse>>
    {
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IUserRepository<User> _userRepository;
        private readonly ISignInRegisterRepository<SignInRegister> _signInRegisterRepository;

        public CreateRefreshLoginCommandHandler(
            IPasswordService passwordService,
            IUserRepository<User> userRepository,
            IMediator mediator,
            ISignInRegisterRepository<SignInRegister> signInRegisterRepository,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _passwordService = passwordService;
            _mediator = mediator;
            _signInRegisterRepository = signInRegisterRepository;
            _userRepository = userRepository;
        }
        public async Task<ResultTask<CreateLoginResponse>> Handle(CreateRefreshLoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                //throw new Exception("falha grave");
                //create the object to be returned on the end of this task
                var result = new ResultTask<CreateLoginResponse>();

                //verify if user exists on Db
                var entity = await _userRepository.GetByEmail(command.Request.Email);
                if (entity == null)
                {
                    result.WithError("Invalid credentials");
                    return result;
                }

                //verify the refresh token
                var lastSignIn = await _signInRegisterRepository.GetLastSignInByUserAsync(entity.Id, command.Request.SignInType);
                if (lastSignIn == null)
                {
                    result.WithError("Invalid request");
                    return result;
                }

                if (string.Compare(lastSignIn.AuthRefreshToken, command.Request.AuthRefreshToken) != 0)
                {
                    result.WithError("Invalid request");
                    return result;
                }

                if (string.Compare(lastSignIn.AuthToken, command.Request.AuthToken) != 0)
                {
                    result.WithError("Invalid request");
                    return result;
                }

                if (DateTime.UtcNow >= lastSignIn.AuthRefreshTokenValidation)
                {
                    result.WithError("Invalid request");
                    return result;
                }

                //Call token generator handler
                var tokenCommand = new CreateTokenCommand(entity);
                var tokenCommandResponse = await _mediator.Send(tokenCommand);

                var dateNow = DateTime.UtcNow;
                var expirationTime = _configuration.GetSection("Settings:AuthRefreshToken:ExpirationTime").Value;
                var expirationTimeInt = Convert.ToInt32(expirationTime, CultureInfo.InvariantCulture);
               
                //create signIn register
                await _signInRegisterRepository.AddAsync(new SignInRegister
                {
                    AuthToken = tokenCommandResponse.Value.Token,
                    AuthRefreshToken = tokenCommandResponse.Value.RefreshToken,
                    SignInType = command.Request.SignInType,
                    UserId = entity.Id, 
                    AuthRefreshTokenValidation = dateNow.AddMinutes(expirationTimeInt)
                });

                result.Value = tokenCommandResponse.Value;
                return await Task.FromResult(result);
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }
    }
}
