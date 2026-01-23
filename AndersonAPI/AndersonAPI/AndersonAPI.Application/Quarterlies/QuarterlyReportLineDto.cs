using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public record QuarterlyReportLineDto
    {
        public QuarterlyReportLineDto()
        {
        }

        public Guid Id { get; set; }
        public int PartnerCount { get; set; }
        public int Headcount { get; set; }
        public int ClientCount { get; set; }
        public int OfficeCount { get; set; }
        public int LawyerCount { get; set; }
        public double EstimatedRevenue { get; set; }
        public Guid CountryId { get; set; }

        public static QuarterlyReportLineDto Create(
            Guid id,
            int partnerCount,
            int headcount,
            int clientCount,
            int officeCount,
            int lawyerCount,
            double estimatedRevenue,
            Guid countryId)
        {
            return new QuarterlyReportLineDto
            {
                Id = id,
                PartnerCount = partnerCount,
                Headcount = headcount,
                ClientCount = clientCount,
                OfficeCount = officeCount,
                LawyerCount = lawyerCount,
                EstimatedRevenue = estimatedRevenue,
                CountryId = countryId
            };
        }
    }
}