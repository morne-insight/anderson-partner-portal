using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.OpportunityTypes.GetOpportunityTypeById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOpportunityTypeByIdQueryHandler : IRequestHandler<GetOpportunityTypeByIdQuery, OpportunityTypeDto>
    {
        private readonly IOpportunityTypeRepository _opportunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetOpportunityTypeByIdQueryHandler(IOpportunityTypeRepository opportunityTypeRepository)
        {
            _opportunityTypeRepository = opportunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<OpportunityTypeDto> Handle(
            GetOpportunityTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var opportunityType = await _opportunityTypeRepository.FindByIdProjectToAsync<OpportunityTypeDto>(request.Id, cancellationToken);
            if (opportunityType is null)
            {
                throw new NotFoundException($"Could not find OpportunityType '{request.Id}'");
            }
            return opportunityType;
        }
    }
}