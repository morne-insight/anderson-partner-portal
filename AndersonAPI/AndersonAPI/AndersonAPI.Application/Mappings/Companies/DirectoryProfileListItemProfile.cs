using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class DirectoryProfileListItemProfile : Profile
    {
        public DirectoryProfileListItemProfile()
        {
            CreateMap<Company, DirectoryProfileListItem>()
                .ForMember(d => d.Capabilities, opt => opt.MapFrom(src => src.Capabilities))
                .ForMember(d => d.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(src => src.Contacts))
                .ForMember(d => d.Industries, opt => opt.MapFrom(src => src.Industries))
                .ForMember(d => d.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes));
        }
    }

    public static class DirectoryProfileListItemMappingExtensions
    {
        public static DirectoryProfileListItem MapToDirectoryProfileListItem(this Company projectFrom, IMapper mapper) => mapper.Map<DirectoryProfileListItem>(projectFrom);

        public static List<DirectoryProfileListItem> MapToDirectoryProfileListItemList(
            this IEnumerable<Company> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToDirectoryProfileListItem(mapper)).ToList();
    }
}