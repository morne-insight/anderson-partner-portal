using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Capabilities
{
    public class CapabilityDtoProfile : Profile
    {
        public CapabilityDtoProfile()
        {
            CreateMap<Capability, CapabilityDto>();
        }
    }

    public static class CapabilityDtoMappingExtensions
    {
        public static CapabilityDto MapToCapabilityDto(this Capability projectFrom, IMapper mapper) => mapper.Map<CapabilityDto>(projectFrom);

        public static List<CapabilityDto> MapToCapabilityDtoList(this IEnumerable<Capability> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToCapabilityDto(mapper)).ToList();
    }
}