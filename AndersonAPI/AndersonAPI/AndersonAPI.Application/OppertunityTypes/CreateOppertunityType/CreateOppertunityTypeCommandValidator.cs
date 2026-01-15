using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.OppertunityTypes.CreateOppertunityType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateOppertunityTypeCommandValidator : AbstractValidator<CreateOppertunityTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateOppertunityTypeCommandValidator()
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