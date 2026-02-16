using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class DirectoryServiceTypeDtoProfile : Profile
    {
        public DirectoryServiceTypeDtoProfile()
        {
            CreateMap<ServiceType, DirectoryServiceTypeDto>();
        }
    }

    public static class DirectoryServiceTypeDtoMappingExtensions
    {
        public static DirectoryServiceTypeDto MapToDirectoryServiceTypeDto(this ServiceType projectFrom, IMapper mapper) => mapper.Map<DirectoryServiceTypeDto>(projectFrom);

        public static List<DirectoryServiceTypeDto> MapToDirectoryServiceTypeDtoList(
            this IEnumerable<ServiceType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToDirectoryServiceTypeDto(mapper)).ToList();
    }
}