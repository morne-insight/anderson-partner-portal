using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyApplicationIdentityUserDtoProfile : Profile
    {
        public CompanyApplicationIdentityUserDtoProfile()
        {
            CreateMap<ApplicationIdentityUser, CompanyApplicationIdentityUserDto>();
        }
    }

    public static class CompanyApplicationIdentityUserDtoMappingExtensions
    {
        public static CompanyApplicationIdentityUserDto MapToCompanyApplicationIdentityUserDto(
            this ApplicationIdentityUser projectFrom,
            IMapper mapper) => mapper.Map<CompanyApplicationIdentityUserDto>(projectFrom);

        public static List<CompanyApplicationIdentityUserDto> MapToCompanyApplicationIdentityUserDtoList(
            this IEnumerable<ApplicationIdentityUser> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyApplicationIdentityUserDto(mapper)).ToList();
    }
}