using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Industries.CreateIndustry
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateIndustryCommandValidator : AbstractValidator<CreateIndustryCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateIndustryCommandValidator()
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