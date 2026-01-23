using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public class QuarterlyReportLineDtoProfile : Profile
    {
        public QuarterlyReportLineDtoProfile()
        {
            CreateMap<ReportLine, QuarterlyReportLineDto>();
        }
    }

    public static class QuarterlyReportLineDtoMappingExtensions
    {
        public static QuarterlyReportLineDto MapToQuarterlyReportLineDto(this ReportLine projectFrom, IMapper mapper) => mapper.Map<QuarterlyReportLineDto>(projectFrom);

        public static List<QuarterlyReportLineDto> MapToQuarterlyReportLineDtoList(
            this IEnumerable<ReportLine> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToQuarterlyReportLineDto(mapper)).ToList();
    }
}