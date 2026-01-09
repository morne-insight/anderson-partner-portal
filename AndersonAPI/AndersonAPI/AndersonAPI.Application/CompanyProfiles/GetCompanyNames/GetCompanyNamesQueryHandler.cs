using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles.GetCompanyNames
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCompanyNamesQueryHandler : IRequestHandler<GetCompanyNamesQuery, List<CompanyNameDto>>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public GetCompanyNamesQueryHandler(ICompanyProfileRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<CompanyNameDto>> Handle(GetCompanyNamesQuery request, CancellationToken cancellationToken)
        {
            var companyNames = await _companyProfileRepository.FindAllProjectToAsync<CompanyNameDto>(cancellationToken);
            return companyNames;
        }
    }
}