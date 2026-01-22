using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.UpdateQuarterly
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateQuarterlyCommandValidator : AbstractValidator<UpdateQuarterlyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateQuarterlyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Quarter)
                .NotNull()
                .IsInEnum();
        }
    }
}