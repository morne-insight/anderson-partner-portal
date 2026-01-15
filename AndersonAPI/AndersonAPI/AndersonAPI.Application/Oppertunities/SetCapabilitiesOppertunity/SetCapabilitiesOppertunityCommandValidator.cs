using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetCapabilitiesOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetCapabilitiesOppertunityCommandValidator : AbstractValidator<SetCapabilitiesOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetCapabilitiesOppertunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.CapabilityIds)
                .NotNull();
        }
    }
}