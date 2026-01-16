using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.SetStateServiceType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateServiceTypeCommandValidator : AbstractValidator<SetStateServiceTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateServiceTypeCommandValidator()
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