using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetHeadOfficeCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetHeadOfficeCompanyCommandValidator : AbstractValidator<SetHeadOfficeCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetHeadOfficeCompanyCommandValidator()
        {
            ConfigureValidationRules();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Depends on user code")]
        private void ConfigureValidationRules()
        {
            // Implement custom validation logic here if required
        }
    }
}