using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetStateOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateOppertunityCommandValidator : AbstractValidator<SetStateOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateOppertunityCommandValidator()
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