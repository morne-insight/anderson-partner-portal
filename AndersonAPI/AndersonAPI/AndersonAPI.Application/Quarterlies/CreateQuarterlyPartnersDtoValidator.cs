using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.FluentValidation.Dtos.DTOValidator", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateQuarterlyPartnersDtoValidator : AbstractValidator<CreateQuarterlyPartnersDto>
    {
        [IntentManaged(Mode.Merge)]
        public CreateQuarterlyPartnersDtoValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Status)
                .NotNull()
                .IsInEnum();

            RuleFor(v => v.Name)
                .NotNull();

            RuleFor(v => v.Description)
                .NotNull();

            RuleFor(v => v.State)
                .NotNull()
                .IsInEnum();
        }
    }
}