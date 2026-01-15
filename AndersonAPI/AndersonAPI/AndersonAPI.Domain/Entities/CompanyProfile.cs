using AndersonAPI.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class CompanyProfile : BaseEntity, IAuditable
    {
        private List<Location> _locations = [];
        private List<Industry> _industries = [];
        private List<Capability> _capabilities = [];
        private List<ServiceType> _serviceTypes = [];
        private List<Contact> _contacts = [];
        private List<ApplicationIdentityUser> _applicationIdentityUsers = [];
        private List<Oppertunity> _oppertunities = [];
        private List<Review> _reviews = [];
        public CompanyProfile(string name, string shortDescription, string description, string websiteUrl, int employeeCount)
        {
            Name = name;
            ShortDescription = shortDescription;
            FullDescription = description;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected CompanyProfile()
        {
            Name = null!;
            ShortDescription = null!;
            FullDescription = null!;
            WebsiteUrl = null!;
        }

        public string Name { get; private set; }

        public string ShortDescription { get; private set; }

        public string FullDescription { get; private set; }

        public string WebsiteUrl { get; private set; }

        public int EmployeeCount { get; private set; }

        public byte[]? Embedding { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid? UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedDate { get; private set; }

        public virtual IReadOnlyCollection<Location> Locations
        {
            get => _locations.AsReadOnly();
            private set => _locations = new List<Location>(value);
        }

        public virtual IReadOnlyCollection<Industry> Industries
        {
            get => _industries.AsReadOnly();
            private set => _industries = new List<Industry>(value);
        }

        public virtual IReadOnlyCollection<Capability> Capabilities
        {
            get => _capabilities.AsReadOnly();
            private set => _capabilities = new List<Capability>(value);
        }

        public virtual IReadOnlyCollection<ServiceType> ServiceTypes
        {
            get => _serviceTypes.AsReadOnly();
            private set => _serviceTypes = new List<ServiceType>(value);
        }

        public virtual IReadOnlyCollection<Contact> Contacts
        {
            get => _contacts.AsReadOnly();
            private set => _contacts = new List<Contact>(value);
        }

        public virtual IReadOnlyCollection<ApplicationIdentityUser> ApplicationIdentityUsers
        {
            get => _applicationIdentityUsers.AsReadOnly();
            private set => _applicationIdentityUsers = new List<ApplicationIdentityUser>(value);
        }

        public virtual IReadOnlyCollection<Oppertunity> Oppertunities
        {
            get => _oppertunities.AsReadOnly();
            private set => _oppertunities = new List<Oppertunity>(value);
        }

        public void Update(string name, string shortDescription, string description, string websiteUrl, int employeeCount)
        {
            Name = name;
            ShortDescription = shortDescription;
            FullDescription = description;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
        }

        public virtual IReadOnlyCollection<Review> Reviews
        {
            get => _reviews.AsReadOnly();
            private set => _reviews = new List<Review>(value);
        }

        public Task<Guid> AddLocation(string name, Guid regionId, Guid countryId, bool isHeadOffice)
        {
            // TODO: Implement AddLocation (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task UpdateLocation(Guid locationId, string name, Guid regionId, Guid countryId, bool isHeadOffice)
        {
            // TODO: Implement UpdateLocation (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task RemoveLocation(Guid locationId)
        {
            // TODO: Implement RemoveLocation (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task SetHeadOffice(Guid locationId)
        {
            // TODO: Implement SetHeadOffice (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task<Guid> AddContact(string firstName, string lastName, string? emailAddress, string? companyPosition)
        {
            // TODO: Implement AddContact (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task UpdateContact(
            Guid contactId,
            string firstName,
            string lastName,
            string? emailAddress,
            string? companyPosition)
        {
            // TODO: Implement UpdateContact (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task RemoveContact(Guid contactId)
        {
            // TODO: Implement RemoveContact (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task UpdateIndustries(IEnumerable<Guid> industryIds)
        {
            // TODO: Implement UpdateIndustries (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void UpdateCapabilities(IEnumerable<Guid> capabilityIds)
        {
            // TODO: Implement UpdateCapabilities (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void UpdateServiceTypes(IEnumerable<Guid> serviceTypeIds)
        {
            // TODO: Implement UpdateServiceTypes (CompanyProfile) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}