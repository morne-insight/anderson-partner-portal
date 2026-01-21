using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.DeleteQuarterly
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class DeleteQuarterlyCommandValidator : AbstractValidator<DeleteQuarterlyCommand>
    {
        [IntentManaged(Mode.Merge)]
        public DeleteQuarterlyCommandValidator()
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