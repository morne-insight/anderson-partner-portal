using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Countries
{
    public class CountryDtoProfile : Profile
    {
        public CountryDtoProfile()
        {
            CreateMap<Country, CountryDto>();
        }
    }

    public static class CountryDtoMappingExtensions
    {
        public static CountryDto MapToCountryDto(this Country projectFrom, IMapper mapper) => mapper.Map<CountryDto>(projectFrom);

        public static List<CountryDto> MapToCountryDtoList(this IEnumerable<Country> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToCountryDto(mapper)).ToList();
    }
}