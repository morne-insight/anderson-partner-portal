using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetServiceTypesOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetServiceTypesOpportunityCommandValidator : AbstractValidator<SetServiceTypesOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetServiceTypesOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.ServiceTypeIds)
                .NotNull();
        }
    }
}