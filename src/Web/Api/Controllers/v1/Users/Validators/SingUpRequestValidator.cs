using CleanTemplate.Api.Controllers.v1.Users.Requests;
using FluentValidation;

namespace CleanTemplate.Api.Controllers.v1.Users.Validators
{
    public class SingUpRequestValidator : AbstractValidator<SingUpRequest>
    {
        public SingUpRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Email)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Password)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.FullName)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Age)
                .NotNull().NotEmpty().GreaterThan(0).WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Gender)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");
        }
    }
}
