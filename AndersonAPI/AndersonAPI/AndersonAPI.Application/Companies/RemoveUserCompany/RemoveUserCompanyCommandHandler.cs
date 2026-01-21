using System.Runtime.CompilerServices;
using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.RemoveUserCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class RemoveUserCompanyCommandHandler : IRequestHandler<RemoveUserCompanyCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationIdentityUserRepository _applicationIdentityUserRepository;
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public RemoveUserCompanyCommandHandler(ICurrentUserService currentUserService, IApplicationIdentityUserRepository applicationIdentityUserRepository, ICompanyRepository companyRepository)
        {
            _currentUserService = currentUserService;
            _applicationIdentityUserRepository = applicationIdentityUserRepository;
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(RemoveUserCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id, cancellationToken);
            if (company == null) throw new NotFoundException($"Could not find Company '{request.Id}'.");

            var currentUser = await _currentUserService.GetAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("Current user is not authenticated.");

            ApplicationIdentityUser? applicationUser = currentUser.Id != null
                ? await _applicationIdentityUserRepository.FindByIdAsync(currentUser.Id.Value.ToString(), cancellationToken)
                : null;

            if (applicationUser == null) throw new NotFoundException("Application user is null.");

            company.RemoveUser(applicationUser);
        }
    }
}