using Core.DTOs.Tenants;
using FluentValidation;

namespace Core.Validators.Tenants
{
    public class TenantDtoValidator : AbstractValidator<TenantDto>
    {
        public TenantDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");
        }
    }
}
