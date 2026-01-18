using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.UpdateOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateOpportunityCommandValidator : AbstractValidator<UpdateOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Title)
                .NotNull();

            RuleFor(v => v.ShortDescription)
                .NotNull();

            RuleFor(v => v.FullDescription)
                .NotNull();

            RuleFor(v => v.Status)
                .NotNull()
                .IsInEnum();
        }
    }
}