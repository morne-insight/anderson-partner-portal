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
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationIdentityUserRepository _applicationIdentityUserRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Merge)]
        public GetInvitesByUserIdQueryHandler(
            IInviteRepository inviteRepository,
            ICurrentUserService currentUserService,
            IApplicationIdentityUserRepository applicationIdentityUserRepository,
            IMapper mapper
            )
        {
            _inviteRepository = inviteRepository;
            _currentUserService = currentUserService;
            _applicationIdentityUserRepository = applicationIdentityUserRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<InviteDto>> Handle(GetInvitesByUserIdQuery request, CancellationToken cancellationToken)
        {
            ApplicationIdentityUser? applicationUser;

            if (string.IsNullOrEmpty(request.Id))
            {
                // Request comes from client
                var currentUser = await _currentUserService.GetAsync();
                if (currentUser == null) throw new UnauthorizedAccessException("Current user is not authenticated.");
                if (!currentUser.Id.HasValue) throw new UnauthorizedAccessException("Current user ID is null.");
                applicationUser = await _applicationIdentityUserRepository.FindByIdAsync(currentUser.Id.Value.ToString(), cancellationToken);

            }
            else
            {
                // Request comes from registration process
                applicationUser = await _applicationIdentityUserRepository.FindByIdAsync(request.Id, cancellationToken);
            }

            if (applicationUser == null) throw new NotFoundException($"User with ID '{request.Id}' was not found.");

            var invites = await _inviteRepository
                .FindAllProjectToAsync<InviteDto>(i => i.Email == applicationUser.Email, cancellationToken);

            return invites;
        }
    }
}