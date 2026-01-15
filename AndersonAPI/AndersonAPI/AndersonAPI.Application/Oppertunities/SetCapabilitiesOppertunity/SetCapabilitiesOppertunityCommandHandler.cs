using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetCapabilitiesOppertunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetCapabilitiesOppertunityCommandHandler : IRequestHandler<SetCapabilitiesOppertunityCommand>
    {
        private readonly IOppertunityRepository _oppertunityRepository;
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public SetCapabilitiesOppertunityCommandHandler(IOppertunityRepository oppertunityRepository, ICapabilityRepository capabilityRepository)
        {
            _oppertunityRepository = oppertunityRepository;
            _capabilityRepository = capabilityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetCapabilitiesOppertunityCommand request, CancellationToken cancellationToken)
        {
            var oppertunity = await _oppertunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (oppertunity is null)
            {
                throw new NotFoundException($"Could not find Oppertunity '{request.Id}'");
            }

            var capabilities = await _capabilityRepository.FindByIdsAsync(request.CapabilityIds.ToArray(), cancellationToken);
            oppertunity.SetCapabilities(capabilities);
        }
    }
}