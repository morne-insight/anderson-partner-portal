using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public static class OpportunityIndustryDtoMappingExtensions
    {
        public static OpportunityIndustryDto MapToOpportunityIndustryDto(this Industry projectFrom, IMapper mapper) => mapper.Map<OpportunityIndustryDto>(projectFrom);

        public static List<OpportunityIndustryDto> MapToOpportunityIndustryDtoList(
            this IEnumerable<Industry> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityIndustryDto(mapper)).ToList();
    }

    public class OpportunityIndustryDtoProfile : Profile
    {
        public OpportunityIndustryDtoProfile()
        {
            CreateMap<Industry, OpportunityIndustryDto>();
        }
    }
}