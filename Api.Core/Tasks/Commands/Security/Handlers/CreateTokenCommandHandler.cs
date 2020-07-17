using Api.Core.Dto.Responses;
using Api.Core.Entities.Security;
using Api.Core.Enums.Security;
using Api.Core.Interfaces.CrossCutting.Services;
using Api.Core.Interfaces.Infra.Repositories.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Core.Tasks.Commands.Security.Handlers
{
    class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, ResultTask<CreateLoginResponse>>
    {
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IAuthTokenService _authTokenService;
        private readonly IUserRepository<User> _userRepository;

        public CreateTokenCommandHandler(
            IPasswordService passwordService,
            IUserRepository<User> userRepository,
            IAuthTokenService authTokenService,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _authTokenService = authTokenService;
            _passwordService = passwordService;
            _userRepository = userRepository;
        }
        public async Task<ResultTask<CreateLoginResponse>> Handle(CreateTokenCommand command, CancellationToken cancellationToken)
        {
            try
            {
                //create the object to be returned on the end of this task
                var result = new ResultTask<CreateLoginResponse>();

                //Values from appSettings
                var configKey = _configuration.GetSection("Settings:AuthToken:Key").Value;
                var configTime = _configuration.GetSection("Settings:AuthToken:ExpirationTime").Value;

                //Claims
                var claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, command.Request.Name),
                    new Claim(ClaimTypes.Email, command.Request.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    //new Claim(EnumPermissions.Perm1.ToString(), true.ToString()),
                    new Claim(EnumPermissions.Perm2.ToString(), true.ToString())
                    };


                //generate new token 
                var newToken = _authTokenService.GenerateToken(claims, configKey, Convert.ToInt32(configTime));

                //add the value of the returned object
                var response = new CreateLoginResponse
                {
                    Token = newToken
                };

                result.Value = response;
                return await Task.FromResult(result);
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }
    }
}
