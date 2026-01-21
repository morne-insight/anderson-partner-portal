using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class ReportLine : BaseEntity
    {
        public ReportLine(Guid quarterlyId,
    int partnerCount = 0,
    int headcount = 0,
    int clientCount = 0,
    int officeCount = 0,
    int lawyerCount = 0,
    double estimatedRevenue = 0,
    EntityState state = EntityState.Enabled)
        {
            QuarterlyId = quarterlyId;
            PartnerCount = partnerCount;
            Headcount = headcount;
            ClientCount = clientCount;
            OfficeCount = officeCount;
            LawyerCount = lawyerCount;
            EstimatedRevenue = estimatedRevenue;
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected ReportLine()
        {
        }

        public int PartnerCount { get; private set; }

        public int Headcount { get; private set; }

        public int ClientCount { get; private set; }

        public int OfficeCount { get; private set; }

        public int LawyerCount { get; private set; }

        public double EstimatedRevenue { get; private set; }

        public Guid QuarterlyId { get; private set; }

        public void Update(
            int partnerCount,
            int headcount,
            int clientCount,
            int officeCount,
            int lawyerCount,
            double estimatedRevenue)
        {
            PartnerCount = partnerCount;
            Headcount = headcount;
            ClientCount = clientCount;
            OfficeCount = officeCount;
            LawyerCount = lawyerCount;
            EstimatedRevenue = estimatedRevenue;
        }
    }
}