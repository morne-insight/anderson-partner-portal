using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.GetMyOpportunities
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetMyOpportunitiesQueryHandler : IRequestHandler<GetMyOpportunitiesQuery, List<OpportunityListItemDto>>
    {
        private readonly IOpportunityRepository _opportunityRepository;

        [IntentManaged(Mode.Merge)]
        public GetMyOpportunitiesQueryHandler(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<OpportunityListItemDto>> Handle(
            GetMyOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {

            // TODO: Implement code to get the current user's opportunities based on their linked companies.

            throw new NotImplementedException();
            //// Get the current user ID from the request context (will be populated by auth middleware)
            //var userId = Guid.Parse("4A8CCC39-95C2-4F7A-B534-C0D166947E43"); // This would normally come from the ClaimsPrincipal

            //// Find all companies that the user is linked to
            //var companyIds = new List<Guid> { 
            //    Guid.Parse("FC968C3A-B5F9-4F3F-9E6D-62FC53845625"),
            //    Guid.Parse("D8A7F2F9-8A87-4D0A-9A45-28C44F20D602") 
            //}; // This would normally come from a query to get the user's companies

            //// If no companies found, return empty list
            //if (!companyIds.Any())
            //{
            //    return new List<OpportunityListItemDto>();
            //}

            //// Find all opportunities linked to those companies
            //var opportunities = await _opportunityRepository.FindOpportunitiesByCompanyIdsProjectToAsync<OpportunityListItemDto>
            //    (companyIds, cancellationToken);

            //return opportunities;
        }
    }
}