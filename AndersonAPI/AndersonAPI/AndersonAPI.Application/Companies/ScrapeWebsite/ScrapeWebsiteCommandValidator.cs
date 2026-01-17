using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.ScrapeWebsite
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class ScrapeWebsiteCommandValidator : AbstractValidator<ScrapeWebsiteCommand>
    {
        [IntentManaged(Mode.Merge)]
        public ScrapeWebsiteCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Url)
                .NotNull();
        }
    }
}