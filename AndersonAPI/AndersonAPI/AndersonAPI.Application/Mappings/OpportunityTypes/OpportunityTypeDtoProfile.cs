using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.OpportunityTypes
{
    public static class OpportunityTypeDtoMappingExtensions
    {
        public static OpportunityTypeDto MapToOpportunityTypeDto(this OpportunityType projectFrom, IMapper mapper) => mapper.Map<OpportunityTypeDto>(projectFrom);

        public static List<OpportunityTypeDto> MapToOpportunityTypeDtoList(
            this IEnumerable<OpportunityType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityTypeDto(mapper)).ToList();
    }

    public class OpportunityTypeDtoProfile : Profile
    {
        public OpportunityTypeDtoProfile()
        {
            CreateMap<OpportunityType, OpportunityTypeDto>();
        }
    }
}