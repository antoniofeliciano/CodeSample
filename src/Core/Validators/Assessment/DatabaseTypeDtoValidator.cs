using Core.DTOs.Assessment;
using FluentValidation;

namespace Core.Validators.Assessment
{
    public class DatabaseTypeDtoValidator : AbstractValidator<DatabaseTypeDto>
    {
        public DatabaseTypeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required")
                .MaximumLength(50).WithMessage("'{PropertyName}' must be a maximum of 50 characters");
        }
    }
}
