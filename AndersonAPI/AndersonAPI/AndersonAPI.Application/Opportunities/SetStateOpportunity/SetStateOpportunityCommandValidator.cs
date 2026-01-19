using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetStateOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateOpportunityCommandValidator : AbstractValidator<SetStateOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.State)
                .NotNull()
                .IsInEnum();
        }
    }
}