using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Capabilities.DeleteCapability
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteCapabilityCommandHandler : IRequestHandler<DeleteCapabilityCommand>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteCapabilityCommandHandler(ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteCapabilityCommand request, CancellationToken cancellationToken)
        {
            var capability = await _capabilityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (capability is null)
            {
                throw new NotFoundException($"Could not find Capability '{request.Id}'");
            }


            _capabilityRepository.Remove(capability);
        }
    }
}