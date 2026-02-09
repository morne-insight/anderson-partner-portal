using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public class DashboardOpportunityDtoProfile : Profile
    {
        public DashboardOpportunityDtoProfile()
        {
            CreateMap<Opportunity, DashboardOpportunityDto>()
                .ForMember(d => d.OpportunityType, opt => opt.MapFrom(src => src.OpportunityType.Name));
        }
    }

    public static class DashboardOpportunityDtoMappingExtensions
    {
        public static DashboardOpportunityDto MapToDashboardOpportunityDto(this Opportunity projectFrom, IMapper mapper) => mapper.Map<DashboardOpportunityDto>(projectFrom);

        public static List<DashboardOpportunityDto> MapToDashboardOpportunityDtoList(
            this IEnumerable<Opportunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToDashboardOpportunityDto(mapper)).ToList();
    }
}