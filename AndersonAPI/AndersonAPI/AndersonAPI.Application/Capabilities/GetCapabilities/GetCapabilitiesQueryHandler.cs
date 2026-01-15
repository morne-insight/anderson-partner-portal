using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Capabilities.GetCapabilities
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCapabilitiesQueryHandler : IRequestHandler<GetCapabilitiesQuery, List<CapabilityDto>>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public GetCapabilitiesQueryHandler(ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<CapabilityDto>> Handle(GetCapabilitiesQuery request, CancellationToken cancellationToken)
        {
            var capabilities = await _capabilityRepository.FindAllProjectToAsync<CapabilityDto>(cancellationToken);
            return capabilities;
        }
    }
}