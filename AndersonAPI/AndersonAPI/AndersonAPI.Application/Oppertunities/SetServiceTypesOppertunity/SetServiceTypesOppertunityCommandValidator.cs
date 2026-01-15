using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetServiceTypesOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetServiceTypesOppertunityCommandValidator : AbstractValidator<SetServiceTypesOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetServiceTypesOppertunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.ServiceTypeIds)
                .NotNull();
        }
    }
}