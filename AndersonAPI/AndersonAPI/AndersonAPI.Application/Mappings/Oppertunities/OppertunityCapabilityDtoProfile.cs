using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public class OppertunityCapabilityDtoProfile : Profile
    {
        public OppertunityCapabilityDtoProfile()
        {
            CreateMap<Capability, OppertunityCapabilityDto>();
        }
    }

    public static class OppertunityCapabilityDtoMappingExtensions
    {
        public static OppertunityCapabilityDto MapToOppertunityCapabilityDto(this Capability projectFrom, IMapper mapper) => mapper.Map<OppertunityCapabilityDto>(projectFrom);

        public static List<OppertunityCapabilityDto> MapToOppertunityCapabilityDtoList(
            this IEnumerable<Capability> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOppertunityCapabilityDto(mapper)).ToList();
    }
}