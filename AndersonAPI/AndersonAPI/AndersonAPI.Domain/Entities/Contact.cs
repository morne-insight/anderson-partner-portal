using AndersonAPI.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Contact : BaseEntity, IAuditable
    {
        public Contact(string firstName, string lastName, string? emailAddress, string? companyPosition, Guid companyId)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            CompanyPosition = companyPosition;
            CompanyId = companyId;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Contact()
        {
            FirstName = null!;
            LastName = null!;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string? EmailAddress { get; private set; }

        public string? CompanyPosition { get; private set; }

        public Guid CompanyId { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid? UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedDate { get; private set; }

        public void Update(string firstName, string lastName, string? emailAddress, string? companyPosition)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            CompanyPosition = companyPosition;
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}