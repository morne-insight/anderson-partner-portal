using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.ServiceTypes
{
    public class ServiceTypeDtoProfile : Profile
    {
        public ServiceTypeDtoProfile()
        {
            CreateMap<ServiceType, ServiceTypeDto>();
        }
    }

    public static class ServiceTypeDtoMappingExtensions
    {
        public static ServiceTypeDto MapToServiceTypeDto(this ServiceType projectFrom, IMapper mapper) => mapper.Map<ServiceTypeDto>(projectFrom);

        public static List<ServiceTypeDto> MapToServiceTypeDtoList(
            this IEnumerable<ServiceType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToServiceTypeDto(mapper)).ToList();
    }
}