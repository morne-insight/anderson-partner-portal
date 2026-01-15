using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.UpdateServiceType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateServiceTypeCommandValidator : AbstractValidator<UpdateServiceTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateServiceTypeCommandValidator()
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