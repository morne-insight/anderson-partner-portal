using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public class OppertunityServiceTypeDtoProfile : Profile
    {
        public OppertunityServiceTypeDtoProfile()
        {
            CreateMap<ServiceType, OppertunityServiceTypeDto>();
        }
    }

    public static class OppertunityServiceTypeDtoMappingExtensions
    {
        public static OppertunityServiceTypeDto MapToOppertunityServiceTypeDto(
            this ServiceType projectFrom,
            IMapper mapper) => mapper.Map<OppertunityServiceTypeDto>(projectFrom);

        public static List<OppertunityServiceTypeDto> MapToOppertunityServiceTypeDtoList(
            this IEnumerable<ServiceType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToOppertunityServiceTypeDto(mapper)).ToList();
    }
}