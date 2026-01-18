using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.UpdateOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateOpportunityCommandHandler : IRequestHandler<UpdateOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateOpportunityCommandHandler(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }

            opportunity.Update(
                request.Title,
                request.ShortDescription,
                request.FullDescription,
                request.Deadline,
                request.OpportunityTypeId,
                request.CountryId,
                request.Status);
        }
    }
}