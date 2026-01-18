using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.OpportunityTypes.DeleteOpportunityType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteOpportunityTypeCommandHandler : IRequestHandler<DeleteOpportunityTypeCommand>
    {
        private readonly IOpportunityTypeRepository _opportunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteOpportunityTypeCommandHandler(IOpportunityTypeRepository opportunityTypeRepository)
        {
            _opportunityTypeRepository = opportunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteOpportunityTypeCommand request, CancellationToken cancellationToken)
        {
            var opportunityType = await _opportunityTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunityType is null)
            {
                throw new NotFoundException($"Could not find OpportunityType '{request.Id}'");
            }


            _opportunityTypeRepository.Remove(opportunityType);
        }
    }
}