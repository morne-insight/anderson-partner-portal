using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public static class OpportunityDtoMappingExtensions
    {
        public static OpportunityDto MapToOpportunityDto(this Opportunity projectFrom, IMapper mapper) => mapper.Map<OpportunityDto>(projectFrom);

        public static List<OpportunityDto> MapToOpportunityDtoList(
            this IEnumerable<Opportunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityDto(mapper)).ToList();
    }

    public class OpportunityDtoProfile : Profile
    {
        public OpportunityDtoProfile()
        {
            CreateMap<Opportunity, OpportunityDto>()
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(d => d.OpportunityType, opt => opt.MapFrom(src => src.OpportunityType.Name))
                .ForMember(d => d.InterestedPartners, opt => opt.MapFrom(src => src.InterestedPartners))
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities))
                .ForMember(d => d.Industries, opt => opt.MapFrom(src => src.Industries))
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes));
        }
    }
}