using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyLocationDtoProfile : Profile
    {
        public CompanyLocationDtoProfile()
        {
            CreateMap<Location, CompanyLocationDto>();
        }
    }

    public static class CompanyLocationDtoMappingExtensions
    {
        public static CompanyLocationDto MapToCompanyLocationDto(this Location projectFrom, IMapper mapper) => mapper.Map<CompanyLocationDto>(projectFrom);

        public static List<CompanyLocationDto> MapToCompanyLocationDtoList(
            this IEnumerable<Location> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyLocationDto(mapper)).ToList();
    }
}