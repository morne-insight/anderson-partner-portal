using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetServiceTypesCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetServiceTypesCompanyCommandValidator : AbstractValidator<SetServiceTypesCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetServiceTypesCompanyCommandValidator()
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