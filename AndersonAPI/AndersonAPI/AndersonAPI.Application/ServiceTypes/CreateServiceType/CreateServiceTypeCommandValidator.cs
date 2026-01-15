using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.CreateServiceType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateServiceTypeCommandValidator : AbstractValidator<CreateServiceTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateServiceTypeCommandValidator()
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