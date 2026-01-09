using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles.GetCompanyProfileById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCompanyProfileByIdQueryHandler : IRequestHandler<GetCompanyProfileByIdQuery, CompanyProfileDto>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public GetCompanyProfileByIdQueryHandler(ICompanyProfileRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<CompanyProfileDto> Handle(
            GetCompanyProfileByIdQuery request,
            CancellationToken cancellationToken)
        {
            var companyProfile = await _companyProfileRepository.FindByIdProjectToAsync<CompanyProfileDto>(request.Id, cancellationToken);
            if (companyProfile is null)
            {
                throw new NotFoundException($"Could not find CompanyProfile '{request.Id}'");
            }
            return companyProfile;
        }
    }
}