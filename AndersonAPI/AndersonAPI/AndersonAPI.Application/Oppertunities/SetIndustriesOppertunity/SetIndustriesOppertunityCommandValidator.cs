using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetIndustriesOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetIndustriesOppertunityCommandValidator : AbstractValidator<SetIndustriesOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetIndustriesOppertunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.IndustryIds)
                .NotNull();
        }
    }
}