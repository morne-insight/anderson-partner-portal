using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.UpdateContactCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateContactCompanyCommandValidator : AbstractValidator<UpdateContactCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateContactCompanyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.FirstName)
                .NotNull();

            RuleFor(v => v.LastName)
                .NotNull();
        }
    }
}