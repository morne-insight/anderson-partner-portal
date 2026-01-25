using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public class OpportunityMessageDtoProfile : Profile
    {
        public OpportunityMessageDtoProfile()
        {
            CreateMap<Message, OpportunityMessageDto>();
        }
    }

    public static class OpportunityMessageDtoMappingExtensions
    {
        public static OpportunityMessageDto MapToOpportunityMessageDto(this Message projectFrom, IMapper mapper) => mapper.Map<OpportunityMessageDto>(projectFrom);

        public static List<OpportunityMessageDto> MapToOpportunityMessageDtoList(
            this IEnumerable<Message> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOpportunityMessageDto(mapper)).ToList();
    }
}