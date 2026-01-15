using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.OppertunityTypes
{
    public class OppertunityTypeDtoProfile : Profile
    {
        public OppertunityTypeDtoProfile()
        {
            CreateMap<OppertunityType, OppertunityTypeDto>();
        }
    }

    public static class OppertunityTypeDtoMappingExtensions
    {
        public static OppertunityTypeDto MapToOppertunityTypeDto(this OppertunityType projectFrom, IMapper mapper) => mapper.Map<OppertunityTypeDto>(projectFrom);

        public static List<OppertunityTypeDto> MapToOppertunityTypeDtoList(
            this IEnumerable<OppertunityType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOppertunityTypeDto(mapper)).ToList();
    }
}