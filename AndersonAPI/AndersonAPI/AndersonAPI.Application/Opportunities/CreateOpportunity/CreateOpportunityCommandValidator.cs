using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.CreateOpportunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateOpportunityCommandValidator : AbstractValidator<CreateOpportunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateOpportunityCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Title)
                .NotNull();

            RuleFor(v => v.ShortDescription)
                .NotNull();

            RuleFor(v => v.FullDescription)
                .NotNull();

            RuleFor(v => v.ServiceTypes)
                .NotNull();

            RuleFor(v => v.Capabilities)
                .NotNull();

            RuleFor(v => v.Industries)
                .NotNull();
        }
    }
}