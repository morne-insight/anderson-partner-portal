using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public record QuarterlyDto
    {
        public QuarterlyDto()
        {
        }

        public ReportQuarter Quarter { get; set; }
        public bool IsSubmitted { get; set; }
        public Guid Id { get; set; }
        public int Year { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public double Revenue { get; set; }
        public int Headcount { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }

        public static QuarterlyDto Create(
            ReportQuarter quarter,
            bool isSubmitted,
            DateTimeOffset submittedDate,
            Guid id,
            int year,
            DateTimeOffset createdDate,
            double revenue,
            int headcount)
        {
            return new QuarterlyDto
            {
                Quarter = quarter,
                IsSubmitted = isSubmitted,
                SubmittedDate = submittedDate
,
                Id = id,
                Year = year,
                CreatedDate = createdDate,
                Revenue = revenue,
                Headcount = headcount
            };
        }
    }
}