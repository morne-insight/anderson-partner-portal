using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerServiceTypeDtoProfile : Profile
    {
        public PartnerServiceTypeDtoProfile()
        {
            CreateMap<ServiceType, PartnerServiceTypeDto>();
        }
    }

    public static class PartnerServiceTypeDtoMappingExtensions
    {
        public static PartnerServiceTypeDto MapToPartnerServiceTypeDto(this ServiceType projectFrom, IMapper mapper) => mapper.Map<PartnerServiceTypeDto>(projectFrom);

        public static List<PartnerServiceTypeDto> MapToPartnerServiceTypeDtoList(
            this IEnumerable<ServiceType> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerServiceTypeDto(mapper)).ToList();
    }
}