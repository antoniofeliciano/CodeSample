using Core.Entities.Authentication;
using FluentValidation;

namespace Core.Validators.Authentication
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");

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
