using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.OppertunityTypes.SetStateOppertunityType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetStateOppertunityTypeCommandHandler : IRequestHandler<SetStateOppertunityTypeCommand>
    {
        private readonly IOppertunityTypeRepository _oppertunityType;

        [IntentManaged(Mode.Merge)]
        public SetStateOppertunityTypeCommandHandler(IOppertunityTypeRepository oppertunityType)
        {
            _oppertunityType = oppertunityType;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SetStateOppertunityTypeCommand request, CancellationToken cancellationToken)
        {
            var oppertunityType = await _oppertunityType.FindByIdAsync(request.Id, cancellationToken);
            if (oppertunityType is null)
            {
                throw new NotFoundException($"Could not find Oppertunity Type '{request.Id}'");
            }

            oppertunityType.SetState(request.State);
        }
    }
}