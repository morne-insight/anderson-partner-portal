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

        [IntentManaged(Mode.Merge)]
        public GetMyCompaniesQueryHandler(ICompanyRepository companyRepository, ICurrentUserService currentUserService)
        {
            _companyRepository = companyRepository;
            _currentUserService = currentUserService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<CompanyProfileDto>> Handle(GetMyCompaniesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("Current user is not authenticated.");

            var companies = await _companyRepository.FindAllProjectToAsync<CompanyProfileDto>(x => x.CreatedBy ==  currentUser.Id, cancellationToken);
            return companies;
        }
    }
}