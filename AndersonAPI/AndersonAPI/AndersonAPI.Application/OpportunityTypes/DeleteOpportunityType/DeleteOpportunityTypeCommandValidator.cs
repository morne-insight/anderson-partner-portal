using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.OpportunityTypes.DeleteOpportunityType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class DeleteOpportunityTypeCommandValidator : AbstractValidator<DeleteOpportunityTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public DeleteOpportunityTypeCommandValidator()
        {
            ConfigureValidationRules();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Depends on user code")]
        private void ConfigureValidationRules()
        {
            // Implement custom validation logic here if required
        }
    }
}