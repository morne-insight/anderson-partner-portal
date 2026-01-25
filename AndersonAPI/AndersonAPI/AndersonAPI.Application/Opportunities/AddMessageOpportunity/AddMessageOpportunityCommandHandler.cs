using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.AddMessageOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AddMessageOpportunityCommandHandler : IRequestHandler<AddMessageOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;

        [IntentManaged(Mode.Merge)]
        public AddMessageOpportunityCommandHandler(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(AddMessageOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdAsync(request.OpportunityId, cancellationToken);
            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.OpportunityId}'");
            }

            opportunity.AddMessage(
                request.OpportunityId,
                request.Content,
                request.CreatedDate,
                request.CreatedByUserId,
                request.CreatedByUser,
                request.CreatedByPartner,
                request.IsOwnMessage);
        }
    }
}