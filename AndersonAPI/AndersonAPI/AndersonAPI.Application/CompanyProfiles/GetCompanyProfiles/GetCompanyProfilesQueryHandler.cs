using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles.GetCompanyProfiles
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCompanyProfilesQueryHandler : IRequestHandler<GetCompanyProfilesQuery, List<CompanyProfileDto>>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public GetCompanyProfilesQueryHandler(ICompanyProfileRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<CompanyProfileDto>> Handle(
            GetCompanyProfilesQuery request,
            CancellationToken cancellationToken)
        {
            var companyProfiles = await _companyProfileRepository.FindAllProjectToAsync<CompanyProfileDto>(cancellationToken);
            return companyProfiles;
        }
    }
}