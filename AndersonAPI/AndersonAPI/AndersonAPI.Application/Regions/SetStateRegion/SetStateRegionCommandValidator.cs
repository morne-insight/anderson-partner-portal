using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Regions.SetStateRegion
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SetStateRegionCommandValidator : AbstractValidator<SetStateRegionCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetStateRegionCommandValidator()
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