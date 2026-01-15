using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Regions.GetRegionById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, RegionDto>
    {
        private readonly IRegionRepository _regionRepository;

        [IntentManaged(Mode.Merge)]
        public GetRegionByIdQueryHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<RegionDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
        {
            var region = await _regionRepository.FindByIdProjectToAsync<RegionDto>(request.Id, cancellationToken);
            if (region is null)
            {
                throw new NotFoundException($"Could not find Region '{request.Id}'");
            }
            return region;
        }
    }
}