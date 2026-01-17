using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.QueryValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.GetPartnersByAI
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class GetPartnersByAIQueryValidator : AbstractValidator<GetPartnersByAIQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetPartnersByAIQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Query)
                .NotNull();
        }
    }
}