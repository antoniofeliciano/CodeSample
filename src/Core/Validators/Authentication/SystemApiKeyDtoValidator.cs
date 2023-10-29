using Core.DTOs.Authentication;
using FluentValidation;

namespace Core.Validators.Authentication
{
    public class SystemApiKeyValidator : AbstractValidator<SystemApiKeyDto>
    {
        public SystemApiKeyValidator()
        {
            RuleFor(x => x.AppName)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");

            RuleFor(x => x.ApiKey)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");

            RuleFor(x => x.ExpirationDate)
                .NotNull()  
                .NotEmpty().WithMessage("'{PropertyName}' is required")
                .GreaterThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("'{PropertyName}' must have at least one day of validity.");
        }
    }
}
