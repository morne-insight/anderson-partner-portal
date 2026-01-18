using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public static class PartnerOpportunityDtoMappingExtensions
    {
        public static PartnerOpportunityDto MapToPartnerOpportunityDto(this Opportunity projectFrom, IMapper mapper) => mapper.Map<PartnerOpportunityDto>(projectFrom);

        public static List<PartnerOpportunityDto> MapToPartnerOpportunityDtoList(
            this IEnumerable<Opportunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerOpportunityDto(mapper)).ToList();
    }

    public class PartnerOpportunityDtoProfile : Profile
    {
        public PartnerOpportunityDtoProfile()
        {
            CreateMap<Opportunity, PartnerOpportunityDto>()
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(d => d.OpportunityType, opt => opt.MapFrom(src => src.OpportunityType.Name));
        }
    }
}