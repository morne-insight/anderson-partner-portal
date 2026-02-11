using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.User
{
    public class UserContactProfile : Profile
    {
        public UserContactProfile()
        {
            CreateMap<Contact, UserContact>()
                .ForMember(d => d.ContactId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(d => d.WebsiteUrl, opt => opt.MapFrom(src => src.Company.WebsiteUrl));
        }
    }

    public static class UserContactMappingExtensions
    {
        public static UserContact MapToUserContact(this Contact projectFrom, IMapper mapper) => mapper.Map<UserContact>(projectFrom);

        public static List<UserContact> MapToUserContactList(this IEnumerable<Contact> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToUserContact(mapper)).ToList();
    }
}