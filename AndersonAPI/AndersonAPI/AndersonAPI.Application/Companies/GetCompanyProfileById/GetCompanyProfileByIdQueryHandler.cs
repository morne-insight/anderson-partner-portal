using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetCompanyProfileById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCompanyProfileByIdQueryHandler : IRequestHandler<GetCompanyProfileByIdQuery, CompanyProfileDto>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public GetCompanyProfileByIdQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<CompanyProfileDto> Handle(
            GetCompanyProfileByIdQuery request,
            CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdProjectToAsync<CompanyProfileDto>(request.Id, cancellationToken);
            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }
            return company;
        }
    }
}