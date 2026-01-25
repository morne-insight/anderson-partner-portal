using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public class OpportunityViewPartnerDtoProfile : Profile
    {
        public OpportunityViewPartnerDtoProfile()
        {
            CreateMap<Company, OpportunityViewPartnerDto>();
        }
    }

    public static class OpportunityViewPartnerDtoMappingExtensions
    {
        public static OpportunityViewPartnerDto MapToOpportunityViewPartnerDto(this Company projectFrom, IMapper mapper) => mapper.Map<OpportunityViewPartnerDto>(projectFrom);

        public static List<OpportunityViewPartnerDto> MapToOpportunityViewPartnerDtoList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityViewPartnerDto(mapper)).ToList();
    }
}