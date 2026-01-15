using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.OppertunityTypes.DeleteOppertunityType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteOppertunityTypeCommandHandler : IRequestHandler<DeleteOppertunityTypeCommand>
    {
        private readonly IOppertunityTypeRepository _oppertunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteOppertunityTypeCommandHandler(IOppertunityTypeRepository oppertunityTypeRepository)
        {
            _oppertunityTypeRepository = oppertunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteOppertunityTypeCommand request, CancellationToken cancellationToken)
        {
            var oppertunityType = await _oppertunityTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (oppertunityType is null)
            {
                throw new NotFoundException($"Could not find OppertunityType '{request.Id}'");
            }


            _oppertunityTypeRepository.Remove(oppertunityType);
        }
    }
}