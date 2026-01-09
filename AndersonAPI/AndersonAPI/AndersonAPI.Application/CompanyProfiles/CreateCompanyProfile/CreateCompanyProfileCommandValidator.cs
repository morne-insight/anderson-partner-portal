using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.CompanyProfiles.CreateCompanyProfile
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateCompanyProfileCommandValidator : AbstractValidator<CreateCompanyProfileCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateCompanyProfileCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Name)
                .NotNull()
                .MaximumLength(160);

            RuleFor(v => v.ShortDescription)
                .NotNull();

            RuleFor(v => v.Description)
                .NotNull();

            RuleFor(v => v.WebsiteUrl)
                .NotNull()
                .MaximumLength(200);
        }
    }
}