using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerIndustryDtoProfile : Profile
    {
        public PartnerIndustryDtoProfile()
        {
            CreateMap<Industry, PartnerIndustryDto>();
        }
    }

    public static class PartnerIndustryDtoMappingExtensions
    {
        public static PartnerIndustryDto MapToPartnerIndustryDto(this Industry projectFrom, IMapper mapper) => mapper.Map<PartnerIndustryDto>(projectFrom);

        public static List<PartnerIndustryDto> MapToPartnerIndustryDtoList(
            this IEnumerable<Industry> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerIndustryDto(mapper)).ToList();
    }
}