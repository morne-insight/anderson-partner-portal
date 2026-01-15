using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.QueryValidator", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.GetServiceTypes
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class GetServiceTypesQueryValidator : AbstractValidator<GetServiceTypesQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetServiceTypesQueryValidator()
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