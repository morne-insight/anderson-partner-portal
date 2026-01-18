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

        [IntentManaged(Mode.Merge)]
        public GetSavedOpportunitiesQueryHandler(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<OpportunityListItemDto>> Handle(
            GetSavedOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {
            var opportunities = await _opportunityRepository.FindAllProjectToAsync<OpportunityListItemDto>(cancellationToken);
            return opportunities;
        }
    }
}