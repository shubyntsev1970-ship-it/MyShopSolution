using FluentValidation;
using MyShop.Api.DTOs.Auth;

namespace MyShop.Api.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Пароль должен содержать большую букву")
            .Matches("[a-z]").WithMessage("Пароль должен содержать маленькую букву")
            .Matches("[0-9]").WithMessage("Пароль должен содержать цифру");
    }
}
