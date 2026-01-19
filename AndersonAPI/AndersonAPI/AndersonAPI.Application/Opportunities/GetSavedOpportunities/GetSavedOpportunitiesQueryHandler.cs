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
        private readonly ICurrentUserService _currentUserService;

        [IntentManaged(Mode.Merge)]
        public GetSavedOpportunitiesQueryHandler(IOpportunityRepository opportunityRepository, ICurrentUserService currentUserService)
        {
            _opportunityRepository = opportunityRepository;
            _currentUserService = currentUserService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<OpportunityListItemDto>> Handle(
            GetSavedOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetAsync();
            if (user == null) throw new UnauthorizedAccessException("The user is not authenticated");

            var userId = user.Id;

            return new List<OpportunityListItemDto>();
            var opportunities = await _opportunityRepository.FindAllProjectToAsync<OpportunityListItemDto>(cancellationToken);
            return opportunities;
        }
    }
}