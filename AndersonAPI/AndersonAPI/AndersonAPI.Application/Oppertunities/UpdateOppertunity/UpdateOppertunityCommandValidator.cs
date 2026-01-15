using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.UpdateOppertunity
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateOppertunityCommandValidator : AbstractValidator<UpdateOppertunityCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateOppertunityCommandValidator()
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
        }
    }
}