using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetCapabilitiesOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetCapabilitiesOpportunityCommandHandler : IRequestHandler<SetCapabilitiesOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public SetCapabilitiesOpportunityCommandHandler(IOpportunityRepository opportunityRepository, ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetCapabilitiesOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }

            var capabilities = await _capabilityRepository.FindByIdsAsync(request.CapabilityIds.ToArray(), cancellationToken);
            opportunity.SetCapabilities(capabilities);
        }
    }
}