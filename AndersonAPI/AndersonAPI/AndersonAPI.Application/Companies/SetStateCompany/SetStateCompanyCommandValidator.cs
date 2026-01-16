using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetStateCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateCompanyCommandValidator : AbstractValidator<SetStateCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateCompanyCommandValidator()
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