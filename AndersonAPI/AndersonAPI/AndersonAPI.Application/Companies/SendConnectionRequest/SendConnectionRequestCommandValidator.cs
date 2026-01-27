using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.FluentValidation.CommandValidator", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SendConnectionRequest
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SendConnectionRequestCommandValidator : AbstractValidator<SendConnectionRequestCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SendConnectionRequestCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Message)
                .NotNull();
        }
    }
}