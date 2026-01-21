using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Invites
{
    public class InviteDtoProfile : Profile
    {
        public InviteDtoProfile()
        {
            CreateMap<Invite, InviteDto>();
        }
    }

    public static class InviteDtoMappingExtensions
    {
        public static InviteDto MapToInviteDto(this Invite projectFrom, IMapper mapper) => mapper.Map<InviteDto>(projectFrom);

        public static List<InviteDto> MapToInviteDtoList(this IEnumerable<Invite> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToInviteDto(mapper)).ToList();
    }
}