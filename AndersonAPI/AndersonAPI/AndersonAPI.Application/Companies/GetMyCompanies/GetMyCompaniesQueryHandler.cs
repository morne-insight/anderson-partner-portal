using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetMyCompanies
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetMyCompaniesQueryHandler : IRequestHandler<GetMyCompaniesQuery, List<CompanyProfileDto>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationIdentityUserRepository _applicationIdentityUserRepository;

        [IntentManaged(Mode.Merge)]
        public GetMyCompaniesQueryHandler(ICompanyRepository companyRepository, ICurrentUserService currentUserService, IApplicationIdentityUserRepository applicationIdentityUserRepository)
        {
            _companyRepository = companyRepository;
            _currentUserService = currentUserService;
            _applicationIdentityUserRepository = applicationIdentityUserRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<CompanyProfileDto>> Handle(GetMyCompaniesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("Current user is not authenticated.");

            var companies = await _companyRepository
                .FindAllProjectToAsync<CompanyProfileDto>(x => x.ApplicationIdentityUsers.Any(u => u.Id == currentUser.Id.ToString()), cancellationToken);

            
            return companies;
        }
    }
}