using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.AddReportPartnerQuarterly
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class AddReportPartnerQuarterlyCommandValidator : AbstractValidator<AddReportPartnerQuarterlyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public AddReportPartnerQuarterlyCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Name)
                .NotNull();

            RuleFor(v => v.State)
                .NotNull()
                .IsInEnum();
        }
    }
}