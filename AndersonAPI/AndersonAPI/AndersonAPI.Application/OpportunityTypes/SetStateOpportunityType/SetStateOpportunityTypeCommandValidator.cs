using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.OpportunityTypes.SetStateOpportunityType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateOpportunityTypeCommandValidator : AbstractValidator<SetStateOpportunityTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateOpportunityTypeCommandValidator()
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