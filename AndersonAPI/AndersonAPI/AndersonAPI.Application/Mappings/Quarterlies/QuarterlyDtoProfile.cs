using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public class QuarterlyDtoProfile : Profile
    {
        public QuarterlyDtoProfile()
        {
            CreateMap<Quarterly, QuarterlyDto>()
                .ForMember(d => d.Revenue, opt => opt.MapFrom(src => src.ReportLines.Sum(s => s.EstimatedRevenue)))
                .ForMember(d => d.Headcount, opt => opt.MapFrom(src => src.ReportLines.Sum(s => s.Headcount)));
        }
    }

    public static class QuarterlyDtoMappingExtensions
    {
        public static QuarterlyDto MapToQuarterlyDto(this Quarterly projectFrom, IMapper mapper) => mapper.Map<QuarterlyDto>(projectFrom);

        public static List<QuarterlyDto> MapToQuarterlyDtoList(this IEnumerable<Quarterly> projectFrom, IMapper mapper) => projectFrom.Select(x => x.MapToQuarterlyDto(mapper)).ToList();
    }
}