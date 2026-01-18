using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public static class OpportunityCapabilityDtoMappingExtensions
    {
        public static OpportunityCapabilityDto MapToOpportunityCapabilityDto(this Capability projectFrom, IMapper mapper) => mapper.Map<OpportunityCapabilityDto>(projectFrom);

        public static List<OpportunityCapabilityDto> MapToOpportunityCapabilityDtoList(
            this IEnumerable<Capability> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityCapabilityDto(mapper)).ToList();
    }

    public class OpportunityCapabilityDtoProfile : Profile
    {
        public OpportunityCapabilityDtoProfile()
        {
            CreateMap<Capability, OpportunityCapabilityDto>();
        }
    }
}