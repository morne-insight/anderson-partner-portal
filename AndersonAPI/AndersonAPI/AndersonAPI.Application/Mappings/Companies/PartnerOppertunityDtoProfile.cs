using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerOppertunityDtoProfile : Profile
    {
        public PartnerOppertunityDtoProfile()
        {
            CreateMap<Oppertunity, PartnerOppertunityDto>()
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(d => d.OppertunityType, opt => opt.MapFrom(src => src.OppertunityType.Name));
        }
    }

    public static class PartnerOppertunityDtoMappingExtensions
    {
        public static PartnerOppertunityDto MapToPartnerOppertunityDto(this Oppertunity projectFrom, IMapper mapper) => mapper.Map<PartnerOppertunityDto>(projectFrom);

        public static List<PartnerOppertunityDto> MapToPartnerOppertunityDtoList(
            this IEnumerable<Oppertunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerOppertunityDto(mapper)).ToList();
    }
}