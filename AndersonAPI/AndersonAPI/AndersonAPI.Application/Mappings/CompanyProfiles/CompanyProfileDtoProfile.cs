using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles
{
    public class CompanyProfileDtoProfile : Profile
    {
        public CompanyProfileDtoProfile()
        {
            CreateMap<CompanyProfile, CompanyProfileDto>();
        }
    }

    public static class CompanyProfileDtoMappingExtensions
    {
        public static CompanyProfileDto MapToCompanyProfileDto(this CompanyProfile projectFrom, IMapper mapper) => mapper.Map<CompanyProfileDto>(projectFrom);

        public static List<CompanyProfileDto> MapToCompanyProfileDtoList(
            this IEnumerable<CompanyProfile> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyProfileDto(mapper)).ToList();
    }
}