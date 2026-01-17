using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetPartnersByAI
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetPartnersByAIQueryHandler : IRequestHandler<GetPartnersByAIQuery, List<PartnerProfileListItem>>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public GetPartnersByAIQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<PartnerProfileListItem>> Handle(
            GetPartnersByAIQuery request,
            CancellationToken cancellationToken)
        {
            var companies = await _companyRepository.FindAllProjectToAsync<PartnerProfileListItem>(cancellationToken);
            return companies;
        }
    }
}