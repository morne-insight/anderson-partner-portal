using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.OpportunityTypes.SetStateOpportunityType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetStateOpportunityTypeCommandHandler : IRequestHandler<SetStateOpportunityTypeCommand>
    {
        private readonly IOpportunityTypeRepository _opportunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public SetStateOpportunityTypeCommandHandler(IOpportunityTypeRepository opportunityTypeRepository)
        {
            _opportunityTypeRepository = opportunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SetStateOpportunityTypeCommand request, CancellationToken cancellationToken)
        {
            var opportunityType = await _opportunityTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunityType is null)
            {
                throw new NotFoundException($"Could not find Opportunity Type '{request.Id}'");
            }

            opportunityType.SetState(request.State);
        }
    }
}