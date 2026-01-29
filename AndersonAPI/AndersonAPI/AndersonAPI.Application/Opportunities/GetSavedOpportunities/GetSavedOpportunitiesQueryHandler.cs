using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.GetSavedOpportunities
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetSavedOpportunitiesQueryHandler : IRequestHandler<GetSavedOpportunitiesQuery, List<OpportunityListItemDto>>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUserService;

        [IntentManaged(Mode.Merge)]
        public GetSavedOpportunitiesQueryHandler(IOpportunityRepository opportunityRepository, ICompanyRepository companyRepository, ICurrentUserService currentUserService)
        {
            _opportunityRepository = opportunityRepository;
            _companyRepository = companyRepository;
            _currentUserService = currentUserService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<OpportunityListItemDto>> Handle(
            GetSavedOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetAsync();
            if (user == null) throw new UnauthorizedAccessException("The user is not authenticated");

            // Get all my companies
            var myCompanies = await _companyRepository
                .FindAllAsync(x =>
                    x.ApplicationIdentityUsers.Any(a => a.Id == user.Id.ToString()),
                    cancellationToken);

            var myCompanyIds = myCompanies.Select(c => c.Id).ToList();

            // Get all opportunities where any of my companies is an interested partner
            var opportunities = await _opportunityRepository
                .FindAllProjectToAsync<OpportunityListItemDto>(
                    x => x.InterestedPartners.Any(p => myCompanyIds.Contains(p.Id)), 
                    cancellationToken);

            return opportunities;
        }
    }
}