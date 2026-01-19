using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerContactDtoProfile : Profile
    {
        public PartnerContactDtoProfile()
        {
            CreateMap<Contact, PartnerContactDto>();
        }
    }

    public static class PartnerContactDtoMappingExtensions
    {
        public static PartnerContactDto MapToPartnerContactDto(this Contact projectFrom, IMapper mapper) => mapper.Map<PartnerContactDto>(projectFrom);

        public static List<PartnerContactDto> MapToPartnerContactDtoList(
            this IEnumerable<Contact> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerContactDto(mapper)).ToList();
    }
}