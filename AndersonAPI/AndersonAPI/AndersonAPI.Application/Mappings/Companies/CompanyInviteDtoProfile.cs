using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class CompanyInviteDtoProfile : Profile
    {
        public CompanyInviteDtoProfile()
        {
            CreateMap<Invite, CompanyInviteDto>();
        }
    }

    public static class CompanyInviteDtoMappingExtensions
    {
        public static CompanyInviteDto MapToCompanyInviteDto(this Invite projectFrom, IMapper mapper) => mapper.Map<CompanyInviteDto>(projectFrom);

        public static List<CompanyInviteDto> MapToCompanyInviteDtoList(
            this IEnumerable<Invite> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToCompanyInviteDto(mapper)).ToList();
    }
}