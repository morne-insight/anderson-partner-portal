using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public static class InterestedPartnerDtoMappingExtensions
    {
        public static InterestedPartnerDto MapToInterestedPartnerDto(this Company projectFrom, IMapper mapper) => mapper.Map<InterestedPartnerDto>(projectFrom);

        public static List<InterestedPartnerDto> MapToInterestedPartnerDtoList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToInterestedPartnerDto(mapper)).ToList();
    }

    public class InterestedPartnerDtoProfile : Profile
    {
        public InterestedPartnerDtoProfile()
        {
            CreateMap<Company, InterestedPartnerDto>();
        }
    }
}