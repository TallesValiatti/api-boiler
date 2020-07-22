using Api.Core.Dto.Responses;
using Api.Core.Entities.Security;
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
    class CreateLoginCommandHandler : IRequestHandler<CreateLoginCommand, ResultTask<CreateLoginResponse>>
    {
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IUserRepository<User> _userRepository;
        private readonly ISignInRegisterRepository<SignInRegister> _signInRegisterRepository;

        public CreateLoginCommandHandler(
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
        public async Task<ResultTask<CreateLoginResponse>> Handle(CreateLoginCommand command, CancellationToken cancellationToken)
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

                //verify the password
                var verifiedPassword = _passwordService.ValidatePassword(command.Request.Password, _configuration.GetSection("Settings:PasswordHashKey").Value, entity.PasswordHash);
                if (!verifiedPassword)
                {
                    result.WithError("Invalid credentials");
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
