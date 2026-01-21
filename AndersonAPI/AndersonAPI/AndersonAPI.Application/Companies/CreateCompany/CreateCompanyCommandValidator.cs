using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.CreateCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateCompanyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Name)
                .NotNull();

            RuleFor(v => v.ShortDescription)
                .NotNull();

            RuleFor(v => v.FullDescription)
                .NotNull();

            RuleFor(v => v.WebsiteUrl)
                .NotNull();

            RuleFor(v => v.Capabilities)
                .NotNull();

            RuleFor(v => v.Industries)
                .NotNull();
        }
    }
}