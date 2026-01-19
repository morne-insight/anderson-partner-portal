using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyContactDtoProfile : Profile
    {
        public CompanyContactDtoProfile()
        {
            CreateMap<Contact, CompanyContactDto>();
        }
    }

    public static class CompanyContactDtoMappingExtensions
    {
        public static CompanyContactDto MapToCompanyContactDto(this Contact projectFrom, IMapper mapper) => mapper.Map<CompanyContactDto>(projectFrom);

        public static List<CompanyContactDto> MapToCompanyContactDtoList(
            this IEnumerable<Contact> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyContactDto(mapper)).ToList();
    }
}