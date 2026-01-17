using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.UpdateMessageOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateMessageOppertunityCommandValidator : AbstractValidator<UpdateMessageOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateMessageOppertunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Content)
                .NotNull();
        }
    }
}