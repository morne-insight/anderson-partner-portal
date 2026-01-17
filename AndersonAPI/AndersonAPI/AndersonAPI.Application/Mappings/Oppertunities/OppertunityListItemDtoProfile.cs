using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public static class OppertunityListItemDtoMappingExtensions
    {
        public static OppertunityListItemDto MapToOppertunityListItemDto(this Oppertunity projectFrom, IMapper mapper) => mapper.Map<OppertunityListItemDto>(projectFrom);

        public static List<OppertunityListItemDto> MapToOppertunityListItemDtoList(
            this IEnumerable<Oppertunity> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOppertunityListItemDto(mapper)).ToList();
    }

    public class OppertunityListItemDtoProfile : Profile
    {
        public OppertunityListItemDtoProfile()
        {
            CreateMap<Oppertunity, OppertunityListItemDto>()
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(d => d.OppertunityType, opt => opt.MapFrom(src => src.OppertunityType.Name))
                .ForMember(d => d.InterestedPartners, opt => opt.MapFrom(src => src.InterestedPartners));
        }
    }
}