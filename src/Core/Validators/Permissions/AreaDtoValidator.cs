using Core.DTOs.Permissions;
using FluentValidation;

namespace Core.Validators.Permissions
{
    public class AreaDtoValidator : AbstractValidator<AreaDto>
    {
        public AreaDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");
        }
    }
}
