using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public static class OpportunityServiceTypeDtoMappingExtensions
    {
        public static OpportunityServiceTypeDto MapToOpportunityServiceTypeDto(
            this ServiceType projectFrom,
            IMapper mapper) => mapper.Map<OpportunityServiceTypeDto>(projectFrom);

        public static List<OpportunityServiceTypeDto> MapToOpportunityServiceTypeDtoList(
            this IEnumerable<ServiceType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityServiceTypeDto(mapper)).ToList();
    }

    public class OpportunityServiceTypeDtoProfile : Profile
    {
        public OpportunityServiceTypeDtoProfile()
        {
            CreateMap<ServiceType, OpportunityServiceTypeDto>();
        }
    }
}