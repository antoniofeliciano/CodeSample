using Core.DTOs.Authentication;
using FluentValidation;

namespace Core.Validators.Authentication
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required")
                .EmailAddress().WithMessage("Invalid e-mail.");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");

        }
    }
}
