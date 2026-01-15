using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.UpdateCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateCompanyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Name)
                .NotNull()
                .MaximumLength(160);

            RuleFor(v => v.ShortDescription)
                .NotNull();

            RuleFor(v => v.Description)
                .NotNull();

            RuleFor(v => v.WebsiteUrl)
                .NotNull()
                .MaximumLength(200);
        }
    }
}