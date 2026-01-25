using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public class OpportunityViewDtoProfile : Profile
    {
        public OpportunityViewDtoProfile()
        {
            CreateMap<Opportunity, OpportunityViewDto>()
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(d => d.CompanyServiceType, opt => opt.MapFrom(src => src.Company.ServiceType != null ? src.Company.ServiceType!.Name : null))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(d => d.OpportunityType, opt => opt.MapFrom(src => src.OpportunityType.Name))
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities))
                .ForMember(d => d.Industries, opt => opt.MapFrom(src => src.Industries))
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes))
                .ForMember(d => d.InterestedPartners, opt => opt.MapFrom(src => src.InterestedPartners));
        }
    }

    public static class OpportunityViewDtoMappingExtensions
    {
        public static OpportunityViewDto MapToOpportunityViewDto(this Opportunity projectFrom, IMapper mapper) => mapper.Map<OpportunityViewDto>(projectFrom);

        public static List<OpportunityViewDto> MapToOpportunityViewDtoList(
            this IEnumerable<Opportunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityViewDto(mapper)).ToList();
    }
}