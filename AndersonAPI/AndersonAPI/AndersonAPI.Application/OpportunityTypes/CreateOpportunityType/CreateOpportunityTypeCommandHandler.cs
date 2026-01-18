using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.OpportunityTypes.CreateOpportunityType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateOpportunityTypeCommandHandler : IRequestHandler<CreateOpportunityTypeCommand, Guid>
    {
        private readonly IOpportunityTypeRepository _opportunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public CreateOpportunityTypeCommandHandler(IOpportunityTypeRepository opportunityTypeRepository)
        {
            _opportunityTypeRepository = opportunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateOpportunityTypeCommand request, CancellationToken cancellationToken)
        {
            var opportunityType = new OpportunityType(
                name: request.Name,
                description: request.Description);

            _opportunityTypeRepository.Add(opportunityType);
            await _opportunityTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return opportunityType.Id;
        }
    }
}