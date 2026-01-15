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
            CreateMap<Oppertunity, OppertunityDto>();
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