using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Countries.CreateCountry
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateCountryCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Name)
                .NotNull();

            RuleFor(v => v.Description)
                .NotNull();
        }
    }
}