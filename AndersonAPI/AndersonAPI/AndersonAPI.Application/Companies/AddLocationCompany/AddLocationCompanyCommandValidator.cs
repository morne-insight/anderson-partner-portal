using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.AddLocationCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class AddLocationCompanyCommandValidator : AbstractValidator<AddLocationCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public AddLocationCompanyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Name)
                .NotNull();
        }
    }
}