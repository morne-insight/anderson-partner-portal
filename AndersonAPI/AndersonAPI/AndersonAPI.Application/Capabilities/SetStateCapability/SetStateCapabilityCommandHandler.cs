using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Capabilities.SetStateCapability
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetStateCapabilityCommandHandler : IRequestHandler<SetStateCapabilityCommand>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public SetStateCapabilityCommandHandler(ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SetStateCapabilityCommand request, CancellationToken cancellationToken)
        {
            var capability = await _capabilityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (capability is null)
            {
                throw new NotFoundException($"Could not find Capability '{request.Id}'");
            }

            capability.SetState(request.State);
        }
    }
}