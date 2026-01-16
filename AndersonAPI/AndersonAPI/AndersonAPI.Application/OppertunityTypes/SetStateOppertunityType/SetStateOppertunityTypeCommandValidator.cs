using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.OppertunityTypes.SetStateOppertunityType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateOppertunityTypeCommandValidator : AbstractValidator<SetStateOppertunityTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateOppertunityTypeCommandValidator()
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