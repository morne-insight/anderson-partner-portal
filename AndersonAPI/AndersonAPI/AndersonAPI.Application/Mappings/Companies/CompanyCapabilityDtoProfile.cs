using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyCapabilityDtoProfile : Profile
    {
        public CompanyCapabilityDtoProfile()
        {
            CreateMap<Capability, CompanyCapabilityDto>();
        }
    }

    public static class CompanyCapabilityDtoMappingExtensions
    {
        public static CompanyCapabilityDto MapToCompanyCapabilityDto(this Capability projectFrom, IMapper mapper) => mapper.Map<CompanyCapabilityDto>(projectFrom);

        public static List<CompanyCapabilityDto> MapToCompanyCapabilityDtoList(
            this IEnumerable<Capability> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyCapabilityDto(mapper)).ToList();
    }
}