using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Capabilities.SetStateCapability
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateCapabilityCommandValidator : AbstractValidator<SetStateCapabilityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateCapabilityCommandValidator()
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