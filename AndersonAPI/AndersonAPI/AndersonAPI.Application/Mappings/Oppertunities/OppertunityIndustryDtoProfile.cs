using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public class OppertunityIndustryDtoProfile : Profile
    {
        public OppertunityIndustryDtoProfile()
        {
            CreateMap<Industry, OppertunityIndustryDto>();
        }
    }

    public static class OppertunityIndustryDtoMappingExtensions
    {
        public static OppertunityIndustryDto MapToOppertunityIndustryDto(this Industry projectFrom, IMapper mapper) => mapper.Map<OppertunityIndustryDto>(projectFrom);

        public static List<OppertunityIndustryDto> MapToOppertunityIndustryDtoList(
            this IEnumerable<Industry> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOppertunityIndustryDto(mapper)).ToList();
    }
}