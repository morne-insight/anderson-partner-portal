using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyServiceSubTypeDtoProfile : Profile
    {
        public CompanyServiceSubTypeDtoProfile()
        {
            CreateMap<ServiceSubType, CompanyServiceSubTypeDto>();
        }
    }

    public static class CompanyServiceSubTypeDtoMappingExtensions
    {
        public static CompanyServiceSubTypeDto MapToCompanyServiceSubTypeDto(
            this ServiceSubType projectFrom,
            IMapper mapper) => mapper.Map<CompanyServiceSubTypeDto>(projectFrom);

        public static List<CompanyServiceSubTypeDto> MapToCompanyServiceSubTypeDtoList(
            this IEnumerable<ServiceSubType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyServiceSubTypeDto(mapper)).ToList();
    }
}