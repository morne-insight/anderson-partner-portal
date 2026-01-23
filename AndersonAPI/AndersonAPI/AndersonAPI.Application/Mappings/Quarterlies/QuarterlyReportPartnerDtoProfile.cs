using AndersonAPI.Domain.Entities;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.DtoMappingProfile", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public class QuarterlyReportPartnerDtoProfile : Profile
    {
        public QuarterlyReportPartnerDtoProfile()
        {
            CreateMap<ReportPartner, QuarterlyReportPartnerDto>();
        }
    }

    public static class QuarterlyReportPartnerDtoMappingExtensions
    {
        public static QuarterlyReportPartnerDto MapToQuarterlyReportPartnerDto(
            this ReportPartner projectFrom,
            IMapper mapper) => mapper.Map<QuarterlyReportPartnerDto>(projectFrom);

        public static List<QuarterlyReportPartnerDto> MapToQuarterlyReportPartnerDtoList(
            this IEnumerable<ReportPartner> projectFrom,
            IMapper mapper) => projectFrom.Select(x => x.MapToQuarterlyReportPartnerDto(mapper)).ToList();
    }
}