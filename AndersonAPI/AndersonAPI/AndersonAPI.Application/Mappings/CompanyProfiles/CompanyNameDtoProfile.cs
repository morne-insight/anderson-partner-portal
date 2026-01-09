using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles
{
    public class CompanyNameDtoProfile : Profile
    {
        public CompanyNameDtoProfile()
        {
            CreateMap<CompanyProfile, CompanyNameDto>();
        }
    }

    public static class CompanyNameDtoMappingExtensions
    {
        public static CompanyNameDto MapToCompanyNameDto(this CompanyProfile projectFrom, IMapper mapper) => mapper.Map<CompanyNameDto>(projectFrom);

        public static List<CompanyNameDto> MapToCompanyNameDtoList(
            this IEnumerable<CompanyProfile> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyNameDto(mapper)).ToList();
    }
}