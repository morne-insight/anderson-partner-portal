using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public class QuarterlyReportDtoProfile : Profile
    {
        public QuarterlyReportDtoProfile()
        {
            CreateMap<Quarterly, QuarterlyReportDto>()
                .ForMember(d => d.Partners, opt => opt.MapFrom(src => src.Partners))
                .ForMember(d => d.ReportLines, opt => opt.MapFrom(src => src.ReportLines));
        }
    }

    public static class QuarterlyReportDtoMappingExtensions
    {
        public static QuarterlyReportDto MapToQuarterlyReportDto(this Quarterly projectFrom, IMapper mapper) => mapper.Map<QuarterlyReportDto>(projectFrom);

        public static List<QuarterlyReportDto> MapToQuarterlyReportDtoList(
            this IEnumerable<Quarterly> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToQuarterlyReportDto(mapper)).ToList();
    }
}