using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.OppertunityTypes.CreateOppertunityType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateOppertunityTypeCommandHandler : IRequestHandler<CreateOppertunityTypeCommand, Guid>
    {
        private readonly IOppertunityTypeRepository _oppertunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public CreateOppertunityTypeCommandHandler(IOppertunityTypeRepository oppertunityTypeRepository)
        {
            _oppertunityTypeRepository = oppertunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateOppertunityTypeCommand request, CancellationToken cancellationToken)
        {
            var oppertunityType = new OppertunityType(
                name: request.Name,
                description: request.Description);

            _oppertunityTypeRepository.Add(oppertunityType);
            await _oppertunityTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return oppertunityType.Id;
        }
    }
}