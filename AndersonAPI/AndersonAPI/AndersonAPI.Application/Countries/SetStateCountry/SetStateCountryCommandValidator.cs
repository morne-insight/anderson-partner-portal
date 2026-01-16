using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Countries.SetStateCountry
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateCountryCommandValidator : AbstractValidator<SetStateCountryCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateCountryCommandValidator()
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