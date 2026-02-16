using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.QueryValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.GetPartnersDirectory
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class GetPartnersDirectoryQueryValidator : AbstractValidator<GetPartnersDirectoryQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetPartnersDirectoryQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {

            RuleFor(v => v.ServiceTypes)
                .NotNull();

            RuleFor(v => v.ServiceSubTypes)
                .NotNull();

            RuleFor(v => v.Industries)
                .NotNull();
            RuleFor(v => v.Capabilities)
                .NotNull();

            RuleFor(v => v.Countries)
                .NotNull();

            RuleFor(v => v.Regions)
                .NotNull();
        }
    }
}