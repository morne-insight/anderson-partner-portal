using Intent.RoslynWeaver.Attributes;
using MediatR;
using AndersonAPI.Domain.Repositories;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles.GetCompanyCount
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCompanyCountQueryHandler : IRequestHandler<GetCompanyCountQuery, int>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public GetCompanyCountQueryHandler(ICompanyProfileRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<int> Handle(GetCompanyCountQuery request, CancellationToken cancellationToken)
        {
            return await _companyProfileRepository.CountAsync(cancellationToken: cancellationToken);
        }
    }
}