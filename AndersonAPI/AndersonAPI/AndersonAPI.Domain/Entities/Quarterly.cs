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

        public DateTimeOffset SubmittedDate { get; private set; }

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
            int partnerCount,
            int headcount,
            int clientCount,
            int officeCount,
            int lawyerCount,
            double estimatedRevenue,
            Guid countryId)
        {
            var line = new ReportLine(partnerCount, headcount, clientCount, officeCount, lawyerCount, estimatedRevenue, countryId);
            _reportLines.Add(line);
        }

        public void UpdateReportLine(
            Guid reprotLineId,
            int partnerCount,
            int headcount,
            int clientCount,
            int officeCount,
            int lawyerCount,
            double estimatedRevenue,
            Guid countryId)
        {
            var line = _reportLines.FirstOrDefault(x => x.Id == reprotLineId);
            if (line == null)
            {
                throw new InvalidOperationException($"Report Line with ID {reprotLineId} not found.");
            }
            line.Update(partnerCount, headcount, clientCount, officeCount, lawyerCount, estimatedRevenue, countryId);
        }

        public void RemoveReportLine(Guid reportLineId)
        {
            var line = _reportLines.FirstOrDefault(x => x.Id == reportLineId);
            if (line == null)
            {
                throw new InvalidOperationException($"Report Line with ID {reportLineId} not found.");
            }
            _reportLines.Remove(line);
        }

        public void AddReportPartner(string name, PartnerStatus status)
        {
            var partner = new ReportPartner(name, status);
            _partners.Add(partner);
        }

        public void UpdateReporPartner(Guid reportPartnerId, string name, PartnerStatus status)
        {
            var partner = _partners.FirstOrDefault(x => x.Id == reportPartnerId);
            if (partner == null)
            {
                throw new InvalidOperationException($"Report Partner with ID {reportPartnerId} not found.");
            }
            partner.Update(name, status);
        }

        public void RemoveReportPartner(Guid reportPartnerId)
        {
            var partner = _partners.FirstOrDefault(x => x.Id == reportPartnerId);
            if (partner == null)
            {
                throw new InvalidOperationException($"Report Partner with ID {reportPartnerId} not found.");
            }
            _partners.Remove(partner);
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}