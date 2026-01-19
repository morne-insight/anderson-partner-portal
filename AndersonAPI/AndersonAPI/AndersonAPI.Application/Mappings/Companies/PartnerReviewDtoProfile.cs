using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public class PartnerReviewDtoProfile : Profile
    {
        public PartnerReviewDtoProfile()
        {
            CreateMap<Review, PartnerReviewDto>();
        }
    }

    public static class PartnerReviewDtoMappingExtensions
    {
        public static PartnerReviewDto MapToPartnerReviewDto(this Review projectFrom, IMapper mapper) => mapper.Map<PartnerReviewDto>(projectFrom);

        public static List<PartnerReviewDto> MapToPartnerReviewDtoList(
            this IEnumerable<Review> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToPartnerReviewDto(mapper)).ToList();
    }
}