using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public static class OpportunityListItemDtoMappingExtensions
    {
        public static OpportunityListItemDto MapToOpportunityListItemDto(this Opportunity projectFrom, IMapper mapper) => mapper.Map<OpportunityListItemDto>(projectFrom);

        public static List<OpportunityListItemDto> MapToOpportunityListItemDtoList(
            this IEnumerable<Opportunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityListItemDto(mapper)).ToList();
    }

    public class OpportunityListItemDtoProfile : Profile
    {
        public OpportunityListItemDtoProfile()
        {
            CreateMap<Opportunity, OpportunityListItemDto>()
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(d => d.OpportunityTypeString, opt => opt.MapFrom(src => src.OpportunityType.Name))
                .ForMember(d => d.InterestedPartners, opt => opt.MapFrom(src => src.InterestedPartners))
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities));
        }
    }
}