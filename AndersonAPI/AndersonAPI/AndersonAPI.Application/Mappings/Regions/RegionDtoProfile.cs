using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Regions
{
    public class RegionDtoProfile : Profile
    {
        public RegionDtoProfile()
        {
            CreateMap<Region, RegionDto>();
        }
    }

    public static class RegionDtoMappingExtensions
    {
        public static RegionDto MapToRegionDto(this Region projectFrom, IMapper mapper) => mapper.Map<RegionDto>(projectFrom);

        public static List<RegionDto> MapToRegionDtoList(this IEnumerable<Region> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToRegionDto(mapper)).ToList();
    }
}