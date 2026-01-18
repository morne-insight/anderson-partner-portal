using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.UpdateFullOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateFullOpportunityCommandHandler : IRequestHandler<UpdateFullOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateFullOpportunityCommandHandler(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateFullOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }

            opportunity.UpdateFull(
                request.Title,
                request.ShortDescription,
                request.FullDescription,
                request.Deadline,
                request.OpportunityTypeId,
                request.CountryId,
                request.ServiceTypes,
                request.Capabilities,
                request.Industries,
                request.Status);
        }
    }
}