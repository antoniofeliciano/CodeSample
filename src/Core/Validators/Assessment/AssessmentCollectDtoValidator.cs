using Core.DTOs.Assessment;
using FluentValidation;

namespace Core.Validators.Assessment
{
    public class AssessmentCollectDtoValidator : AbstractValidator<AssessmentCollectDto>
    {
        public AssessmentCollectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required")
                .MaximumLength(50).WithMessage("'{PropertyName}' must be a maximum of 50 characters");

            RuleFor(x => x.Details)
                .MaximumLength(300).WithMessage("'{PropertyName}' must be a maximum of 300 characters");

            RuleFor(x => x.ClientName)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");

            RuleFor(x => x.CollectResult)
                .NotNull()
                .NotEmpty().WithMessage("'{PropertyName}' is required");
        }
    }
}
