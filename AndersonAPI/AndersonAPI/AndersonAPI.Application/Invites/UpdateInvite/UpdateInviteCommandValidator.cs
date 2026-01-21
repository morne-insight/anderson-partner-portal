using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Invites.UpdateInvite
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class UpdateInviteCommandValidator : AbstractValidator<UpdateInviteCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateInviteCommandValidator()
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