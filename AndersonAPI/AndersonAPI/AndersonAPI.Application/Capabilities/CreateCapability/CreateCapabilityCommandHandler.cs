using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Capabilities.CreateCapability
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateCapabilityCommandHandler : IRequestHandler<CreateCapabilityCommand, Guid>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public CreateCapabilityCommandHandler(ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateCapabilityCommand request, CancellationToken cancellationToken)
        {
            var capability = new Capability(
                name: request.Name,
                description: request.Description);

            _capabilityRepository.Add(capability);
            await _capabilityRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return capability.Id;
        }
    }
}