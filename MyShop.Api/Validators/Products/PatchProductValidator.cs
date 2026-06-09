using FluentValidation;
using MyShop.Api.DTOs.Products;

namespace MyShop.Api.Validators.Products;

public class PatchProductValidator : AbstractValidator<PatchProductDto>
{
    public PatchProductValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(255)
            .When(x => x.Name != null);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price.HasValue);

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Stock.HasValue);
    }
}
