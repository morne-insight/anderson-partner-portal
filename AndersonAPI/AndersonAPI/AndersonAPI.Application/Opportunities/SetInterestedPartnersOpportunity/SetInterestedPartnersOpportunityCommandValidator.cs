using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetInterestedPartnersOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetInterestedPartnersOpportunityCommandValidator : AbstractValidator<SetInterestedPartnersOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetInterestedPartnersOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.CompanyIds)
                .NotNull();
        }
    }
}