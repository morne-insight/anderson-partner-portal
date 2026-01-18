using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetStatusOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStatusOpportunityCommandValidator : AbstractValidator<SetStatusOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStatusOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.StatusOpportunityStatus)
                .NotNull()
                .IsInEnum();
        }
    }
}