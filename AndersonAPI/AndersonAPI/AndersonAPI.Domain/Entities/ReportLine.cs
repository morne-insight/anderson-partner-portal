using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class ReportLine : BaseEntity
    {
        public ReportLine(int partnerCount,
    int headcount,
    int clientCount,
    int officeCount,
    int lawyerCount,
    double estimatedRevenue,
    Guid countryId,
    EntityState state = EntityState.Enabled)
        {
            PartnerCount = partnerCount;
            Headcount = headcount;
            ClientCount = clientCount;
            OfficeCount = officeCount;
            LawyerCount = lawyerCount;
            EstimatedRevenue = estimatedRevenue;
            CountryId = countryId;
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected ReportLine()
        {
            Country = null!;
        }

        public int PartnerCount { get; private set; }

        public int Headcount { get; private set; }

        public int ClientCount { get; private set; }

        public int OfficeCount { get; private set; }

        public int LawyerCount { get; private set; }

        public double EstimatedRevenue { get; private set; }

        public Guid QuarterlyId { get; private set; }

        public Guid CountryId { get; private set; }

        public virtual Country Country { get; private set; }

        public void Update(
            int partnerCount,
            int headcount,
            int clientCount,
            int officeCount,
            int lawyerCount,
            double estimatedRevenue,
            Guid countryId)
        {
            PartnerCount = partnerCount;
            Headcount = headcount;
            ClientCount = clientCount;
            OfficeCount = officeCount;
            LawyerCount = lawyerCount;
            EstimatedRevenue = estimatedRevenue;
            CountryId = countryId;
        }
    }
}