using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Invites.GetInviteById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetInviteByIdQueryHandler : IRequestHandler<GetInviteByIdQuery, InviteDto>
    {
        private readonly IInviteRepository _inviteRepository;

        [IntentManaged(Mode.Merge)]
        public GetInviteByIdQueryHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<InviteDto> Handle(GetInviteByIdQuery request, CancellationToken cancellationToken)
        {
            var invite = await _inviteRepository.FindByIdProjectToAsync<InviteDto>(request.Id, cancellationToken);
            if (invite is null)
            {
                throw new NotFoundException($"Could not find Invite '{request.Id}'");
            }
            return invite;
        }
    }
}