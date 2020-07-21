using Api.Core.Enums.Security;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dto.Requests.Validators
{
    public class CreateRefreshLoginRequestValidator : AbstractValidator<CreateRefreshLoginRequest>
    {
        public CreateRefreshLoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email must be valid");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("Email must be valid");

            RuleFor(x => x.AuthToken)
               .NotEmpty()
               .WithMessage("AuthToken must be valid");

            RuleFor(x => x.AuthToken)
                .NotNull()
                .WithMessage("AuthToken must be valid");

            RuleFor(x => x.AuthRefreshToken)
               .NotEmpty()
               .WithMessage("AuthRefreshToken must be valid");

            RuleFor(x => x.AuthRefreshToken)
                .NotNull()
                .WithMessage("AuthRefreshToken must be valid");

            RuleFor(x => x.SignInType)
                .IsInEnum()
                .WithMessage("Enum SignInType must be valid");
        }
    }
}
