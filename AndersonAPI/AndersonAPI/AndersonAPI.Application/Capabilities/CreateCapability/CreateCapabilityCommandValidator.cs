using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Capabilities.CreateCapability
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateCapabilityCommandValidator : AbstractValidator<CreateCapabilityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateCapabilityCommandValidator()
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