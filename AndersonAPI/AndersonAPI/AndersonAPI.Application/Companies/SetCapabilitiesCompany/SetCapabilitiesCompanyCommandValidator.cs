using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetCapabilitiesCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetCapabilitiesCompanyCommandValidator : AbstractValidator<SetCapabilitiesCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetCapabilitiesCompanyCommandValidator()
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