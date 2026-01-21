using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Invites.GetInvitesByUserId
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetInvitesByUserIdQueryHandler : IRequestHandler<GetInvitesByUserIdQuery, List<InviteDto>>
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        [IntentManaged(Mode.Merge)]
        public GetInvitesByUserIdQueryHandler(IInviteRepository inviteRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _inviteRepository = inviteRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<InviteDto>> Handle(GetInvitesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("Current user is not authenticated.");

            var invites = await _inviteRepository
                .FindAllProjectToAsync<InviteDto>(i => i.Email == currentUser.Name, cancellationToken);

            return invites;
        }
    }
}