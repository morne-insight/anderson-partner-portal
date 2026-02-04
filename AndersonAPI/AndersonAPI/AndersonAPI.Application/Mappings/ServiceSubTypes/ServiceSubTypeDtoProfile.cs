using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.ServiceSubTypes
{
    public class ServiceSubTypeDtoProfile : Profile
    {
        public ServiceSubTypeDtoProfile()
        {
            CreateMap<ServiceSubType, ServiceSubTypeDto>();
        }
    }

    public static class ServiceSubTypeDtoMappingExtensions
    {
        public static ServiceSubTypeDto MapToServiceSubTypeDto(this ServiceSubType projectFrom, IMapper mapper) => mapper.Map<ServiceSubTypeDto>(projectFrom);

        public static List<ServiceSubTypeDto> MapToServiceSubTypeDtoList(
            this IEnumerable<ServiceSubType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToServiceSubTypeDto(mapper)).ToList();
    }
}