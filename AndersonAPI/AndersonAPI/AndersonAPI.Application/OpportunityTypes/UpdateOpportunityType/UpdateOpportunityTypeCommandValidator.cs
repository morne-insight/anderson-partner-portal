using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.OpportunityTypes.UpdateOpportunityType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateOpportunityTypeCommandValidator : AbstractValidator<UpdateOpportunityTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateOpportunityTypeCommandValidator()
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