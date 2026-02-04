using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerServiceSubTypeDtoProfile : Profile
    {
        public PartnerServiceSubTypeDtoProfile()
        {
            CreateMap<ServiceSubType, PartnerServiceSubTypeDto>();
        }
    }

    public static class PartnerServiceSubTypeDtoMappingExtensions
    {
        public static PartnerServiceSubTypeDto MapToPartnerServiceSubTypeDto(
            this ServiceSubType projectFrom,
            IMapper mapper) => mapper.Map<PartnerServiceSubTypeDto>(projectFrom);

        public static List<PartnerServiceSubTypeDto> MapToPartnerServiceSubTypeDtoList(
            this IEnumerable<ServiceSubType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerServiceSubTypeDto(mapper)).ToList();
    }
}