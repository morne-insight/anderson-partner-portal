using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetServiceSubTypeCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetServiceSubTypeCompanyCommandValidator : AbstractValidator<SetServiceSubTypeCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetServiceSubTypeCompanyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.ServiceSubTypeIds)
                .NotNull();
        }
    }
}