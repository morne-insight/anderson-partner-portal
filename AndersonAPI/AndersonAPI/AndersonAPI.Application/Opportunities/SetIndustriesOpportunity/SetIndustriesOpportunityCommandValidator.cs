using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetIndustriesOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetIndustriesOpportunityCommandValidator : AbstractValidator<SetIndustriesOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetIndustriesOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.IndustryIds)
                .NotNull();
        }
    }
}