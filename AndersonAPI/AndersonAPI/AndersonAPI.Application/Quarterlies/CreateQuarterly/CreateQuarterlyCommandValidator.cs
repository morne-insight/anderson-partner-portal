using AndersonAPI.Application.Common.Validation;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.CreateQuarterly
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateQuarterlyCommandValidator : AbstractValidator<CreateQuarterlyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateQuarterlyCommandValidator(IValidatorProvider provider)
        {
            ConfigureValidationRules(provider);
        }

        private void ConfigureValidationRules(IValidatorProvider provider)
        {
            RuleFor(v => v.Partners)
                .NotNull()
                .ForEach(x => x.SetValidator(provider.GetValidator<CreateQuarterlyPartnersDto>()!));

            RuleFor(v => v.Reports)
                .NotNull()
                .ForEach(x => x.SetValidator(provider.GetValidator<CreateQuarterlyReportsDto>()!));

            RuleFor(v => v.State)
                .NotNull()
                .IsInEnum();
        }
    }
}