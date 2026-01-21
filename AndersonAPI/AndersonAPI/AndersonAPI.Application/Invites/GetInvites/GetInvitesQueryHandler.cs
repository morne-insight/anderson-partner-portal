using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Invites.GetInvites
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetInvitesQueryHandler : IRequestHandler<GetInvitesQuery, List<InviteDto>>
    {
        private readonly IInviteRepository _inviteRepository;

        [IntentManaged(Mode.Merge)]
        public GetInvitesQueryHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<InviteDto>> Handle(GetInvitesQuery request, CancellationToken cancellationToken)
        {
            var invites = await _inviteRepository.FindAllProjectToAsync<InviteDto>(cancellationToken);
            return invites;
        }
    }
}