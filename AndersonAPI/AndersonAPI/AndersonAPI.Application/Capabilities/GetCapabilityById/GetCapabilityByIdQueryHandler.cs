using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Capabilities.GetCapabilityById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCapabilityByIdQueryHandler : IRequestHandler<GetCapabilityByIdQuery, CapabilityDto>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        [IntentManaged(Mode.Merge)]
        public GetCapabilityByIdQueryHandler(ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<CapabilityDto> Handle(GetCapabilityByIdQuery request, CancellationToken cancellationToken)
        {
            var capability = await _capabilityRepository.FindByIdProjectToAsync<CapabilityDto>(request.Id, cancellationToken);
            if (capability is null)
            {
                throw new NotFoundException($"Could not find Capability '{request.Id}'");
            }
            return capability;
        }
    }
}