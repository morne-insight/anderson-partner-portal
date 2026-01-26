using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public static class PartnerProfileDtoMappingExtensions
    {
        public static PartnerProfileDto MapToPartnerProfileDto(this Company projectFrom, IMapper mapper) => mapper.Map<PartnerProfileDto>(projectFrom);

        public static List<PartnerProfileDto> MapToPartnerProfileDtoList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerProfileDto(mapper)).ToList();
    }

    public class PartnerProfileDtoProfile : Profile
    {
        public PartnerProfileDtoProfile()
        {
            CreateMap<Company, PartnerProfileDto>()
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(src => src.Contacts))
                .ForMember(d => d.Industries, opt => opt.MapFrom(src => src.Industries))
                .ForMember(d => d.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(d => d.Opportunities, opt => opt.MapFrom(src => src.Opportunities))
                .ForMember(d => d.ServiceTypeName, opt => opt.MapFrom(src => src.ServiceType != null ? src.ServiceType!.Name : null));
        }
    }
}