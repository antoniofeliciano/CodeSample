using Core.DTOs.Permissions;
using FluentValidation;

namespace Core.Validators.Authentication
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
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

            RuleFor(x => x.RoleId)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");
           
            RuleFor(x => x.TenantId)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");

        }
    }
}
