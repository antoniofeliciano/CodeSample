using Core.DTOs.Assessment;
using FluentValidation;

namespace Core.Validators.Assessment
{
    public class AssessmentQueryDtoValidator : AbstractValidator<AssessmentQueryDto>
    {
        public AssessmentQueryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required")
                .MaximumLength(50).WithMessage("'{PropertyName}' must be a maximum of 50 characters");

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required")
                .MaximumLength(300).WithMessage("'{PropertyName}' must be a maximum of 300 characters");

            RuleFor(x => x.QueryString)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");
        }
    }
}
