using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanySearchDtoProfile : Profile
    {
        public CompanySearchDtoProfile()
        {
            CreateMap<Company, CompanySearchDto>();
        }
    }

    public static class CompanySearchDtoMappingExtensions
    {
        public static CompanySearchDto MapToCompanySearchDto(this Company projectFrom, IMapper mapper) => mapper.Map<CompanySearchDto>(projectFrom);

        public static List<CompanySearchDto> MapToCompanySearchDtoList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanySearchDto(mapper)).ToList();
    }
}