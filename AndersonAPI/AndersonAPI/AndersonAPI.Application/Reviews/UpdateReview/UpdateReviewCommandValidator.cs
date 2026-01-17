using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Reviews.UpdateReview
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateReviewCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Comment)
                .NotNull();
        }
    }
}