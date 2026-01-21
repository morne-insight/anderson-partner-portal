using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Invites.UpdateInvite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateInviteCommandHandler : IRequestHandler<UpdateInviteCommand>
    {
        private readonly IInviteRepository _inviteRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateInviteCommandHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateInviteCommand request, CancellationToken cancellationToken)
        {
            var invite = await _inviteRepository.FindByIdAsync(request.Id, cancellationToken);
            if (invite is null)
            {
                throw new NotFoundException($"Could not find Invite '{request.Id}'");
            }

            invite.Update(request.Email);
        }
    }
}