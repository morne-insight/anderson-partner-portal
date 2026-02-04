using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.User
{
    public static class UserInviteDtoMappingExtensions
    {
        public static UserInviteDto MapToUserInviteDto(this Invite projectFrom, IMapper mapper) => mapper.Map<UserInviteDto>(projectFrom);

        public static List<UserInviteDto> MapToUserInviteDtoList(this IEnumerable<Invite> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToUserInviteDto(mapper)).ToList();
    }

    public class UserInviteDtoProfile : Profile
    {
        public UserInviteDtoProfile()
        {
            CreateMap<Invite, UserInviteDto>()
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(d => d.CompanyShortDescription, opt => opt.MapFrom(src => src.Company.ShortDescription))
                .ForMember(d => d.CompanyWebsiteUrl, opt => opt.MapFrom(src => src.Company.WebsiteUrl))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Company.Id));
        }
    }
}