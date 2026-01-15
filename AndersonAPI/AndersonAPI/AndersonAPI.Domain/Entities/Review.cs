using AndersonAPI.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Review : BaseEntity, IAuditable
    {
        private List<Company> _companies = [];

        public Review(string comment, int rating, string applicationIdentityUserId, Guid reviewerCompanyId)
        {
            Comment = comment;
            Rating = rating;
            ApplicationIdentityUserId = applicationIdentityUserId;
            ReviewerCompanyId = reviewerCompanyId;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Review()
        {
            Comment = null!;
            ApplicationIdentityUserId = null!;
            ReviewerCompany = null!;
            ApplicationIdentityUser = null!;
        }

        public string Comment { get; private set; }

        public int Rating { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid? UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedDate { get; private set; }

        public string ApplicationIdentityUserId { get; private set; }

        public Guid ReviewerCompanyId { get; private set; }

        public virtual IReadOnlyCollection<Company> Companies
        {
            get => _companies.AsReadOnly();
            private set => _companies = new List<Company>(value);
        }

        public virtual Company ReviewerCompany { get; private set; }

        public virtual ApplicationIdentityUser ApplicationIdentityUser { get; private set; }

        public void Update(string comment, int rating)
        {
            Comment = comment;
            Rating = rating;
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}