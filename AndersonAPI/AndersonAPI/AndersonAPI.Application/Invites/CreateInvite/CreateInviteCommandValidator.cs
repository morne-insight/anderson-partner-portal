using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Invites.CreateInvite
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CreateInviteCommandValidator : AbstractValidator<CreateInviteCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateInviteCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Email)
                .NotNull()
                .MaximumLength(200);
        }
    }
}