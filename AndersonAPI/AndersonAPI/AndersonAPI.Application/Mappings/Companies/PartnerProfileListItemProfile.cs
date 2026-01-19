using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerProfileListItemProfile : Profile
    {
        public PartnerProfileListItemProfile()
        {
            CreateMap<Company, PartnerProfileListItem>()
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities))
                .ForMember(d => d.Locations, opt => opt.MapFrom(src => src.Locations.Where(l => l.IsHeadOffice)))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(src => src.Contacts));
        }
    }

    public static class PartnerProfileListItemMappingExtensions
    {
        public static PartnerProfileListItem MapToPartnerProfileListItem(this Company projectFrom, IMapper mapper) => mapper.Map<PartnerProfileListItem>(projectFrom);

        public static List<PartnerProfileListItem> MapToPartnerProfileListItemList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerProfileListItem(mapper)).ToList();
    }
}