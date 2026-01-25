using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Reviews
{
    public class ReviewDtoProfile : Profile
    {
        public ReviewDtoProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(src => src.ApplicationIdentityUserId))
                .ForMember(d => d.CreatedByUserName, opt => opt.MapFrom(src => src.ApplicationIdentityUser.Name))
                .ForMember(d => d.ReviewerCompanyName, opt => opt.MapFrom(src => src.ReviewerCompany.Name));
        }
    }

    public static class ReviewDtoMappingExtensions
    {
        public static ReviewDto MapToReviewDto(this Review projectFrom, IMapper mapper) => mapper.Map<ReviewDto>(projectFrom);

        public static List<ReviewDto> MapToReviewDtoList(this IEnumerable<Review> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToReviewDto(mapper)).ToList();
    }
}