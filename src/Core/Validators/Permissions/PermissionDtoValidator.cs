using Core.DTOs.Permissions;
using FluentValidation;

namespace Core.Validators.Permissions
{
    public class PermissionDtoValidator : AbstractValidator<PermissionDto>
    {
        public PermissionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");
        }
    }
}
