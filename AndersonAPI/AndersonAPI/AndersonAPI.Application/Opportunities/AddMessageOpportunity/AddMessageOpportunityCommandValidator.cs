using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.AddMessageOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class AddMessageOpportunityCommandValidator : AbstractValidator<AddMessageOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public AddMessageOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Content)
                .NotNull();

            RuleFor(v => v.CreatedByUser)
                .NotNull();
        }
    }
}