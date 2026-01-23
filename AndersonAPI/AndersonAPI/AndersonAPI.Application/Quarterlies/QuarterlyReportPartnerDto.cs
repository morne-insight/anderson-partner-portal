using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public record QuarterlyReportPartnerDto
    {
        public QuarterlyReportPartnerDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public PartnerStatus Status { get; set; }
        public string Name { get; set; }

        public static QuarterlyReportPartnerDto Create(Guid id, PartnerStatus status, string name)
        {
            return new QuarterlyReportPartnerDto
            {
                Id = id,
                Status = status,
                Name = name
            };
        }
    }
}