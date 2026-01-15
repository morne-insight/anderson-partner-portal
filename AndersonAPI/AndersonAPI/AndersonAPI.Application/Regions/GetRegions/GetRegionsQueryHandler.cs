using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Regions.GetRegions
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, List<RegionDto>>
    {
        private readonly IRegionRepository _regionRepository;

        [IntentManaged(Mode.Merge)]
        public GetRegionsQueryHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<RegionDto>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var regions = await _regionRepository.FindAllProjectToAsync<RegionDto>(cancellationToken);
            return regions;
        }
    }
}