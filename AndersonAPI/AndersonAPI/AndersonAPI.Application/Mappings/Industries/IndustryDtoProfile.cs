using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Industries
{
    public class IndustryDtoProfile : Profile
    {
        public IndustryDtoProfile()
        {
            CreateMap<Industry, IndustryDto>();
        }
    }

    public static class IndustryDtoMappingExtensions
    {
        public static IndustryDto MapToIndustryDto(this Industry projectFrom, IMapper mapper) => mapper.Map<IndustryDto>(projectFrom);

        public static List<IndustryDto> MapToIndustryDtoList(this IEnumerable<Industry> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToIndustryDto(mapper)).ToList();
    }
}