using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Industries.SetStateIndustry
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateIndustryCommandValidator : AbstractValidator<SetStateIndustryCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateIndustryCommandValidator()
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