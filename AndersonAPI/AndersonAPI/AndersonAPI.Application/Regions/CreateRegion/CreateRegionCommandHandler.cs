using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Regions.CreateRegion
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, Guid>
    {
        private readonly IRegionRepository _regionRepository;

        [IntentManaged(Mode.Merge)]
        public CreateRegionCommandHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
        {
            var region = new Region(
                name: request.Name,
                description: request.Description);

            _regionRepository.Add(region);
            await _regionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return region.Id;
        }
    }
}