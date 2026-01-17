using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyIndustryDtoProfile : Profile
    {
        public CompanyIndustryDtoProfile()
        {
            CreateMap<Industry, CompanyIndustryDto>();
        }
    }

    public static class CompanyIndustryDtoMappingExtensions
    {
        public static CompanyIndustryDto MapToCompanyIndustryDto(this Industry projectFrom, IMapper mapper) => mapper.Map<CompanyIndustryDto>(projectFrom);

        public static List<CompanyIndustryDto> MapToCompanyIndustryDtoList(
            this IEnumerable<Industry> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyIndustryDto(mapper)).ToList();
    }
}