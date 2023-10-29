using Core.DTOs.Permissions;
using FluentValidation;

namespace Core.Validators.Permissions
{
    public class InterfaceDtoValidator : AbstractValidator<InterfaceDto>
    {
        public InterfaceDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");
        }
    }
}
