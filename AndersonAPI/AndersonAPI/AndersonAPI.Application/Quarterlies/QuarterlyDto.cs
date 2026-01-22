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

        public static QuarterlyDto Create(ReportQuarter quarter, bool isSubmitted, Guid id)
        {
            return new QuarterlyDto
            {
                Quarter = quarter,
                IsSubmitted = isSubmitted,
                Id = id
            };
        }
    }
}