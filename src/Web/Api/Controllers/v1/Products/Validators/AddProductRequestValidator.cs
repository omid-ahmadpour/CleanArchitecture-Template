using Api.Controllers.v1.Products.Requests;
using FluentValidation;

namespace Api.Controllers.v1.Products.Validators
{
    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Price)
                .NotNull().NotEmpty().GreaterThan(0).WithMessage("{PropertyName} is not valid");
        }
    }
}
