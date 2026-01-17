using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetPartnerProfileById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetPartnerProfileByIdQueryHandler : IRequestHandler<GetPartnerProfileByIdQuery, PartnerProfile>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public GetPartnerProfileByIdQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<PartnerProfile> Handle(GetPartnerProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdProjectToAsync<PartnerProfile>(request.Id, cancellationToken);
            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }
            return company;
        }
    }
}