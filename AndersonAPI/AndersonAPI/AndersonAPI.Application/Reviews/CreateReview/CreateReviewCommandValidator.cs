using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Reviews.CreateReview
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateReviewCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Comment)
                .NotNull();

            RuleFor(v => v.ApplicationIdentityUserId)
                .NotNull();

            RuleFor(v => v.State)
                .NotNull()
                .IsInEnum();
        }
    }
}