using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dto.Requests.Validators
{
    public class CreateLoginRequestValidator : AbstractValidator<CreateLoginRequest>
    {
        public CreateLoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email must be valid");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("Email must be valid");

            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("Password must be valid");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("Password must be valid");
        }
    }
}
