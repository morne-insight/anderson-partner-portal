using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public class DashboardPartnerDtoProfile : Profile
    {
        public DashboardPartnerDtoProfile()
        {
            CreateMap<Company, DashboardPartnerDto>()
                .ForMember(d => d.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes));
        }
    }

    public static class DashboardPartnerDtoMappingExtensions
    {
        public static DashboardPartnerDto MapToDashboardPartnerDto(this Company projectFrom, IMapper mapper) => mapper.Map<DashboardPartnerDto>(projectFrom);

        public static List<DashboardPartnerDto> MapToDashboardPartnerDtoList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToDashboardPartnerDto(mapper)).ToList();
    }
}