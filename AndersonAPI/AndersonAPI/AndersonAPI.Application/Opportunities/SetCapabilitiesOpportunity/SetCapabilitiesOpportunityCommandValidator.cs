using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetCapabilitiesOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetCapabilitiesOpportunityCommandValidator : AbstractValidator<SetCapabilitiesOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetCapabilitiesOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.CapabilityIds)
                .NotNull();
        }
    }
}