using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetCompanies
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<DirectoryProfileListItem>>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public GetCompaniesQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<DirectoryProfileListItem>> Handle(
            GetCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            var companies = await _companyRepository.FindAllProjectToAsync<DirectoryProfileListItem>(cancellationToken);
            return companies;
        }
    }
}