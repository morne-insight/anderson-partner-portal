using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.FluentValidation.Dtos.DTOValidator", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateQuarterlyReportsDtoValidator : AbstractValidator<CreateQuarterlyReportsDto>
    {
        [IntentManaged(Mode.Merge)]
        public CreateQuarterlyReportsDtoValidator()
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