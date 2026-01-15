using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetInterestedPartnersOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetInterestedPartnersOppertunityCommandValidator : AbstractValidator<SetInterestedPartnersOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetInterestedPartnersOppertunityCommandValidator()
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