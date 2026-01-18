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

        [IntentManaged(Mode.Merge)]
        public GetOpportunitiesQueryHandler(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<OpportunityListItemDto>> Handle(
            GetOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {
            var opportunities = await _opportunityRepository.FindAllProjectToAsync<OpportunityListItemDto>(cancellationToken);
            return opportunities;
        }
    }
}