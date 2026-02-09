using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public class DashboardLocationDtoProfile : Profile
    {
        public DashboardLocationDtoProfile()
        {
            CreateMap<Location, DashboardLocationDto>()
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Region, opt => opt.MapFrom(src => src.Region.Name));
        }
    }

    public static class DashboardLocationDtoMappingExtensions
    {
        public static DashboardLocationDto MapToDashboardLocationDto(this Location projectFrom, IMapper mapper) => mapper.Map<DashboardLocationDto>(projectFrom);

        public static List<DashboardLocationDto> MapToDashboardLocationDtoList(
            this IEnumerable<Location> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToDashboardLocationDto(mapper)).ToList();
    }
}