using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Invites.CreateInvite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateInviteCommandHandler : IRequestHandler<CreateInviteCommand, Guid>
    {
        private readonly IInviteRepository _inviteRepository;

        [IntentManaged(Mode.Merge)]
        public CreateInviteCommandHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateInviteCommand request, CancellationToken cancellationToken)
        {
            var invite = new Invite(
                email: request.Email,
                companyId: request.CompanyId);

            _inviteRepository.Add(invite);
            await _inviteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return invite.Id;
        }
    }
}