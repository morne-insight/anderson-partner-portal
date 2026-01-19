using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyServiceTypeDtoProfile : Profile
    {
        public CompanyServiceTypeDtoProfile()
        {
            CreateMap<ServiceType, CompanyServiceTypeDto>();
        }
    }

    public static class CompanyServiceTypeDtoMappingExtensions
    {
        public static CompanyServiceTypeDto MapToCompanyServiceTypeDto(this ServiceType projectFrom, IMapper mapper) => mapper.Map<CompanyServiceTypeDto>(projectFrom);

        public static List<CompanyServiceTypeDto> MapToCompanyServiceTypeDtoList(
            this IEnumerable<ServiceType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyServiceTypeDto(mapper)).ToList();
    }
}