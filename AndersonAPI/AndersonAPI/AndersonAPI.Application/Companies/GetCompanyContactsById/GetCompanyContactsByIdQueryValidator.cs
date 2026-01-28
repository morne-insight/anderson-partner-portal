using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.QueryValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.GetCompanyContactsById
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class GetCompanyContactsByIdQueryValidator : AbstractValidator<GetCompanyContactsByIdQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetCompanyContactsByIdQueryValidator()
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