using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyDtoProfile : Profile
    {
        public CompanyDtoProfile()
        {
            CreateMap<Company, CompanyDto>();
        }
    }

    public static class CompanyDtoMappingExtensions
    {
        public static CompanyDto MapToCompanyDto(this Company projectFrom, IMapper mapper) => mapper.Map<CompanyDto>(projectFrom);

        public static List<CompanyDto> MapToCompanyDtoList(this IEnumerable<Company> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToCompanyDto(mapper)).ToList();
    }
}