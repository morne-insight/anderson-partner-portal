using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.UpdateMessageOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateMessageOpportunityCommandValidator : AbstractValidator<UpdateMessageOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateMessageOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Content)
                .NotNull();
        }
    }
}