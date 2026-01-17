using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyProfileDtoProfile : Profile
    {
        public CompanyProfileDtoProfile()
        {
            CreateMap<Company, CompanyProfileDto>()
                .ForMember(d => d.ApplicationIdentityUsers, opt => opt.MapFrom(src => src.ApplicationIdentityUsers))
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities))
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes))
                .ForMember(d => d.Industries, opt => opt.MapFrom(src => src.Industries))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(src => src.Contacts))
                .ForMember(d => d.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(d => d.Invites, opt => opt.MapFrom(src => src.Invites));
        }
    }

    public static class CompanyProfileDtoMappingExtensions
    {
        public static CompanyProfileDto MapToCompanyProfileDto(this Company projectFrom, IMapper mapper) => mapper.Map<CompanyProfileDto>(projectFrom);

        public static List<CompanyProfileDto> MapToCompanyProfileDtoList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyProfileDto(mapper)).ToList();
    }
}