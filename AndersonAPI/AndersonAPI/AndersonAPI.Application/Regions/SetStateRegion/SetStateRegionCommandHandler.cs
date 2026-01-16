using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Regions.SetStateRegion
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetStateRegionCommandHandler : IRequestHandler<SetStateRegionCommand>
    {
        private readonly IRegionRepository _regionRepository;

        [IntentManaged(Mode.Merge)]
        public SetStateRegionCommandHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SetStateRegionCommand request, CancellationToken cancellationToken)
        {
            var region = await _regionRepository.FindByIdAsync(request.Id, cancellationToken);
            if (region is null)
            {
                throw new NotFoundException($"Could not find Region '{request.Id}'");
            }

            region.SetState(request.State);
        }
    }
}