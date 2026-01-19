using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.GetOpportunities
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOpportunitiesQueryHandler : IRequestHandler<GetOpportunitiesQuery, List<OpportunityListItemDto>>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUserService;

        [IntentManaged(Mode.Merge)]
        public GetOpportunitiesQueryHandler(IOpportunityRepository opportunityRepository, ICurrentUserService currentUserService, ICompanyRepository companyRepository)
        {
            _opportunityRepository = opportunityRepository;
            _currentUserService = currentUserService;
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<OpportunityListItemDto>> Handle(
            GetOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetAsync();
            if (user == null) throw new UnauthorizedAccessException("The user is not authenticated");

            var userId = user.Id;

            // Find all companies that the user is linked to
            var companies = await _companyRepository
                .FindAllAsync(c => c.ApplicationIdentityUsers.Any(u => u.Id == userId.ToString()), cancellationToken);

            // If no companies found, return empty list
            if (companies == null || !companies.Any())
            {
                return new List<OpportunityListItemDto>();
            }

            // Get the company IDs
            var companyIds = companies.Select(c => c.Id).ToList();

            // Find all opportunities linked to those companies
            var opportunities = await _opportunityRepository
                .FindAllProjectToAsync<OpportunityListItemDto>(o => !companyIds.Contains(o.CompanyId), cancellationToken);

            return opportunities;
        }
    }
}