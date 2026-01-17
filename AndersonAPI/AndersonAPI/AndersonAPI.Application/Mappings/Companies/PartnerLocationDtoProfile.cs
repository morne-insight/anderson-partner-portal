using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerLocationDtoProfile : Profile
    {
        public PartnerLocationDtoProfile()
        {
            CreateMap<Location, PartnerLocationDto>();
        }
    }

    public static class PartnerLocationDtoMappingExtensions
    {
        public static PartnerLocationDto MapToPartnerLocationDto(this Location projectFrom, IMapper mapper) => mapper.Map<PartnerLocationDto>(projectFrom);

        public static List<PartnerLocationDto> MapToPartnerLocationDtoList(
            this IEnumerable<Location> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerLocationDto(mapper)).ToList();
    }
}