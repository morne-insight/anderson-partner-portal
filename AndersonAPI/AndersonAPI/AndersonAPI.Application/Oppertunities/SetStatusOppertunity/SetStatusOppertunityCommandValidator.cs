using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetStatusOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStatusOppertunityCommandValidator : AbstractValidator<SetStatusOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStatusOppertunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Status)
                .NotNull()
                .IsInEnum();
        }
    }
}