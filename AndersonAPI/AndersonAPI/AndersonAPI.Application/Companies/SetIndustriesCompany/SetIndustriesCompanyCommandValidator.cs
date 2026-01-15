using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetIndustriesCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetIndustriesCompanyCommandValidator : AbstractValidator<SetIndustriesCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetIndustriesCompanyCommandValidator()
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