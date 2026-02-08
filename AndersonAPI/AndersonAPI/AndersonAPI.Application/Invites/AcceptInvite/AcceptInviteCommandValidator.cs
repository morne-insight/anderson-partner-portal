using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Invites.AcceptInvite
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class AcceptInviteCommandValidator : AbstractValidator<AcceptInviteCommand>
    {
        [IntentManaged(Mode.Merge)]
        public AcceptInviteCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.UserId)
                .NotNull();
        }
    }
}