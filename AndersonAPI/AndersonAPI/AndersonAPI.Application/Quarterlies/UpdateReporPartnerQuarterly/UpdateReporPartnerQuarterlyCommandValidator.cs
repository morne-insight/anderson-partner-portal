using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.UpdateReporPartnerQuarterly
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateReporPartnerQuarterlyCommandValidator : AbstractValidator<UpdateReporPartnerQuarterlyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateReporPartnerQuarterlyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Name)
                .NotNull();
        }
    }
}