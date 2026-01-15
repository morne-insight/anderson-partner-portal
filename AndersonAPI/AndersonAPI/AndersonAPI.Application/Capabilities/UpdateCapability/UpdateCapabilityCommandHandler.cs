using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Capabilities.UpdateCapability
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateCapabilityCommandHandler : IRequestHandler<UpdateCapabilityCommand>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateCapabilityCommandHandler(ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateCapabilityCommand request, CancellationToken cancellationToken)
        {
            var capability = await _capabilityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (capability is null)
            {
                throw new NotFoundException($"Could not find Capability '{request.Id}'");
            }

            capability.Update(request.Name, request.Description);
        }
    }
}