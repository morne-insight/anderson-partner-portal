using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Invites.AcceptInvite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AcceptInviteCommandHandler : IRequestHandler<AcceptInviteCommand>
    {
        [IntentManaged(Mode.Merge)]
        public AcceptInviteCommandHandler()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implement Handle (AcceptInviteCommandHandler) functionality
            throw new NotImplementedException("Your implementation here...");
        }
    }
}