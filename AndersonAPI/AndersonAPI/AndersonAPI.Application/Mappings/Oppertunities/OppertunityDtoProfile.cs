using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public class OppertunityDtoProfile : Profile
    {
        public OppertunityDtoProfile()
        {
            CreateMap<Oppertunity, OppertunityDto>()
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(d => d.OppertunityType, opt => opt.MapFrom(src => src.OppertunityType.Name))
                .ForMember(d => d.InterestedPartners, opt => opt.MapFrom(src => src.InterestedPartners))
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities))
                .ForMember(d => d.Industries, opt => opt.MapFrom(src => src.Industries))
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes));
        }
    }

    public static class OppertunityDtoMappingExtensions
    {
        public static OppertunityDto MapToOppertunityDto(this Oppertunity projectFrom, IMapper mapper) => mapper.Map<OppertunityDto>(projectFrom);

        public static List<OppertunityDto> MapToOppertunityDtoList(
            this IEnumerable<Oppertunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOppertunityDto(mapper)).ToList();
    }
}