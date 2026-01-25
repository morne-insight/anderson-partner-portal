using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.GetOpportunityViewById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOpportunityViewByIdQueryHandler : IRequestHandler<GetOpportunityViewByIdQuery, OpportunityViewDto>
    {
        private readonly IOpportunityRepository _opportunityRepository;

        [IntentManaged(Mode.Merge)]
        public GetOpportunityViewByIdQueryHandler(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<OpportunityViewDto> Handle(
            GetOpportunityViewByIdQuery request,
            CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdProjectToAsync<OpportunityViewDto>(request.Id, cancellationToken);
            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }
            return opportunity;
        }
    }
}