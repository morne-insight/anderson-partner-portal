using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.UpdateLocationCompany
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateLocationCompanyCommandValidator : AbstractValidator<UpdateLocationCompanyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateLocationCompanyCommandValidator()
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