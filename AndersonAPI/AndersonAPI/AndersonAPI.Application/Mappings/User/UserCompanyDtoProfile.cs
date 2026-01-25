using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.User
{
    public static class UserCompanyDtoMappingExtensions
    {
        public static UserCompanyDto MapToUserCompanyDto(this Company projectFrom, IMapper mapper) => mapper.Map<UserCompanyDto>(projectFrom);

        public static List<UserCompanyDto> MapToUserCompanyDtoList(this IEnumerable<Company> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToUserCompanyDto(mapper)).ToList();
    }

    public class UserCompanyDtoProfile : Profile
    {
        public UserCompanyDtoProfile()
        {
            CreateMap<Company, UserCompanyDto>();
        }
    }
}