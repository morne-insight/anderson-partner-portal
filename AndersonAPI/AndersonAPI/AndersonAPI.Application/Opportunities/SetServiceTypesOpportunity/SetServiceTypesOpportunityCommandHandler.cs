using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetServiceTypesOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetServiceTypesOpportunityCommandHandler : IRequestHandler<SetServiceTypesOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public SetServiceTypesOpportunityCommandHandler(IOpportunityRepository opportunityRepository, IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetServiceTypesOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }

            var serviceTypes = await _serviceTypeRepository.FindByIdsAsync(request.ServiceTypeIds.ToArray(), cancellationToken);
            opportunity.SetServiceTypes(serviceTypes);
        }
    }
}