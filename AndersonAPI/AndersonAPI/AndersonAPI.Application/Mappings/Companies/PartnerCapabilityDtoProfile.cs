using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerCapabilityDtoProfile : Profile
    {
        public PartnerCapabilityDtoProfile()
        {
            CreateMap<Capability, PartnerCapabilityDto>();
        }
    }

    public static class PartnerCapabilityDtoMappingExtensions
    {
        public static PartnerCapabilityDto MapToPartnerCapabilityDto(this Capability projectFrom, IMapper mapper) => mapper.Map<PartnerCapabilityDto>(projectFrom);

        public static List<PartnerCapabilityDto> MapToPartnerCapabilityDtoList(
            this IEnumerable<Capability> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerCapabilityDto(mapper)).ToList();
    }
}