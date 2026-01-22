using AndersonAPI.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Quarterly : BaseEntity, IAuditable
    {
        private List<ReportPartner> _partners = [];
        private List<ReportLine> _reportLines = [];

        public Quarterly(int year,
            ReportQuarter quarter,
            Guid companyId,
            IEnumerable<ReportPartner> partners,
            IEnumerable<ReportLine> reports,
            EntityState state = EntityState.Enabled)
        {
            Year = year;
            Quarter = quarter;
            CompanyId = companyId;
            _partners = new List<ReportPartner>(partners);
            _reportLines = new List<ReportLine>(reports);
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Quarterly()
        {
            Company = null!;
        }

        public int Year { get; private set; }

        public ReportQuarter Quarter { get; private set; }

        public Guid CompanyId { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid? UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedDate { get; private set; }

        public bool IsSubmitted { get; private set; }

        public virtual Company Company { get; private set; }

        public virtual IReadOnlyCollection<ReportPartner> Partners
        {
            get => _partners.AsReadOnly();
            private set => _partners = new List<ReportPartner>(value);
        }

        public virtual IReadOnlyCollection<ReportLine> ReportLines
        {
            get => _reportLines.AsReadOnly();
            private set => _reportLines = new List<ReportLine>(value);
        }

        public void Update(int year, ReportQuarter quarter)
        {
            Year = year;
            Quarter = quarter;
        }

        public void SetSubmitted(bool isSubmitted)
        {
            IsSubmitted = isSubmitted;
        }

        public void AddReportLine(
            Guid quarterlyId,
            int partnerCount = 0,
            int headcount = 0,
            int clientCount = 0,
            int officeCount = 0,
            int lawyerCount = 0,
            double estimatedRevenue = 0)
        {
            // TODO: Implement AddReportLine (Quarterly) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void UpdateReportLine(
            Guid reprotLineId,
            int partnerCount,
            int headcount,
            int clientCount,
            int officeCount,
            int lawyerCount,
            double estimatedRevenue)
        {
            // TODO: Implement UpdateReportLine (Quarterly) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void RemoveReportLine(Guid reportLineId)
        {
            // TODO: Implement RemoveReportLine (Quarterly) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void AddReportPartner(Guid quaterlyId, string name, EntityState state)
        {
            // TODO: Implement AddReportPartner (Quarterly) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void UpdateReporPartner(Guid reportPartnerId, string name)
        {
            // TODO: Implement UpdateReporPartner (Quarterly) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void RemoveReportPartner(Guid reportPartnerId)
        {
            // TODO: Implement RemoveReportPartner (Quarterly) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}