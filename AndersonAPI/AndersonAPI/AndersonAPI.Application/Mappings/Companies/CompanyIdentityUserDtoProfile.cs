using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Identity;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public static class CompanyIdentityUserDtoMappingExtensions
    {
        public static CompanyIdentityUserDto MapToCompanyIdentityUserDto(
                this ApplicationIdentityUser projectFrom,
                IMapper mapper) => mapper.Map<CompanyIdentityUserDto>(projectFrom);

        public static List<CompanyIdentityUserDto> MapToCompanyIdentityUserDtoList(
                this IEnumerable<ApplicationIdentityUser> projectFrom,
                IMapper mapper) => projectFrom.Select(x => x.MapToCompanyIdentityUserDto(mapper)).ToList();
    }

    public class CompanyIdentityUserDtoProfile : Profile
    {
        [IntentManaged(Mode.Ignore)]
        public CompanyIdentityUserDtoProfile()
        {
            CreateMap<ApplicationIdentityUser, CompanyIdentityUserDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => Guid.Parse(s.Id)))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.UserName))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email));
        }
    }
}