using CleanTemplate.Api.Controllers.v1.Products.Requests;
using FluentValidation;

namespace CleanTemplate.Api.Controllers.v1.Products.Validators
{
    public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
    {
        public GetProductsRequestValidator()
        {
            RuleFor(x => x.Page)
                .NotNull().NotEmpty().GreaterThan(0).WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.PageSize)
                .NotNull().NotEmpty().GreaterThan(0).WithMessage("{PropertyName} is not valid");
        }
    }
}
