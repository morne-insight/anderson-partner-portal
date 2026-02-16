using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public class DashboardServiceTypeDtoProfile : Profile
    {
        public DashboardServiceTypeDtoProfile()
        {
            CreateMap<ServiceType, DashboardServiceTypeDto>();
        }
    }

    public static class DashboardServiceTypeDtoMappingExtensions
    {
        public static DashboardServiceTypeDto MapToDashboardServiceTypeDto(this ServiceType projectFrom, IMapper mapper) => mapper.Map<DashboardServiceTypeDto>(projectFrom);

        public static List<DashboardServiceTypeDto> MapToDashboardServiceTypeDtoList(
            this IEnumerable<ServiceType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToDashboardServiceTypeDto(mapper)).ToList();
    }
}