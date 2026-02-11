using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Invites.AcceptInvite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AcceptInviteCommandHandler : IRequestHandler<AcceptInviteCommand>
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationIdentityUserRepository _applicationIdentityUserRepository;

        [IntentManaged(Mode.Merge)]
        public AcceptInviteCommandHandler(
            IInviteRepository inviteRepository,
            ICompanyRepository companyRepository,
            ICurrentUserService currentUserService,
            IApplicationIdentityUserRepository applicationIdentityUserRepository)
        {
            _inviteRepository = inviteRepository;
            _companyRepository = companyRepository;
            _currentUserService = currentUserService;
            _applicationIdentityUserRepository = applicationIdentityUserRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
        {
            var invite = await _inviteRepository.FindByIdAsync(request.Id, cancellationToken);
            if (invite == null)
            {
                throw new NotFoundException($"Could not find Invite with id '{request.Id}'");
            }

            var applicationIdentityUser = await _applicationIdentityUserRepository.FindAsync(x => x.Id == request.UserId, cancellationToken);
            if (applicationIdentityUser == null)
            {
                throw new NotFoundException($"Could not find User with id '{request.UserId}'");
            }

            if (applicationIdentityUser.Email != invite.Email)
            {
                throw new UnauthorizedAccessException($"The user with id '{request.UserId}' does not have permission to accept this invite");
            }

            var company = await _companyRepository.FindByIdAsync(invite.CompanyId, cancellationToken);
            if (company == null)
            {
                throw new NotFoundException($"Could not find Company with id '{invite.CompanyId}'");
            }

            company.AddUser(applicationIdentityUser);
            _inviteRepository.Remove(invite);

        }
    }
}