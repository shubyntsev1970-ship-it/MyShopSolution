using FluentValidation;
using MyShop.Api.DTOs.Products;

namespace MyShop.Api.Validators.Products;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название товара обязательно")
            .MaximumLength(255).WithMessage("Название не может быть длиннее 255 символов");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Остаток не может быть отрицательным");
    }
}
