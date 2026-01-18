using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.OpportunityTypes.GetOpportunityTypes
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOpportunityTypesQueryHandler : IRequestHandler<GetOpportunityTypesQuery, List<OpportunityTypeDto>>
    {
        private readonly IOpportunityTypeRepository _opportunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetOpportunityTypesQueryHandler(IOpportunityTypeRepository opportunityTypeRepository)
        {
            _opportunityTypeRepository = opportunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<OpportunityTypeDto>> Handle(
            GetOpportunityTypesQuery request,
            CancellationToken cancellationToken)
        {
            var opportunityTypes = await _opportunityTypeRepository.FindAllProjectToAsync<OpportunityTypeDto>(cancellationToken);
            return opportunityTypes;
        }
    }
}