using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public record QuarterlyReportDto
    {
        public QuarterlyReportDto()
        {
            Partners = null!;
            ReportLines = null!;
        }

        public int Year { get; set; }
        public ReportQuarter Quarter { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public List<QuarterlyReportPartnerDto> Partners { get; set; }
        public List<QuarterlyReportLineDto> ReportLines { get; set; }

        public static QuarterlyReportDto Create(
            int year,
            ReportQuarter quarter,
            DateTimeOffset createdDate,
            bool isSubmitted,
            DateTimeOffset submittedDate,
            List<QuarterlyReportPartnerDto> partners,
            List<QuarterlyReportLineDto> reportLines)
        {
            return new QuarterlyReportDto
            {
                Year = year,
                Quarter = quarter,
                CreatedDate = createdDate,
                IsSubmitted = isSubmitted,
                SubmittedDate = submittedDate,
                Partners = partners,
                ReportLines = reportLines
            };
        }
    }
}