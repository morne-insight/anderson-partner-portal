using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.AddMessageOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class AddMessageOppertunityCommandValidator : AbstractValidator<AddMessageOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public AddMessageOppertunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Content)
                .NotNull();

            RuleFor(v => v.CreatedByUser)
                .NotNull();
        }
    }
}