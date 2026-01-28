using AndersonAPI.Domain.Repositories;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetCompanyContactsById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCompanyContactsByIdQueryHandler : IRequestHandler<GetCompanyContactsByIdQuery, List<CompanyContactDto>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Merge)]
        public GetCompanyContactsByIdQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<List<CompanyContactDto>> Handle(
            GetCompanyContactsByIdQuery request,
            CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id, queryOptions => queryOptions.Include(c => c.Contacts), cancellationToken);
            return company?.Contacts.MapToCompanyContactDtoList(_mapper) ?? new List<CompanyContactDto>();
        }
    }
}