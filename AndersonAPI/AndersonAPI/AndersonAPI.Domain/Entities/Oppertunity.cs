using AndersonAPI.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Oppertunity : BaseEntity, IAuditable
    {
        private List<ServiceType> _serviceTypes = [];
        private List<Capability> _capabilities = [];
        private List<Industry> _industries = [];
        private List<CompanyProfile> _interestedPartners = [];

        public Oppertunity(string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId,
            Guid companyProfileId)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
            CompanyProfileId = companyProfileId;
        }

        public Oppertunity(string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId,
            IEnumerable<ServiceType> serviceTypes,
            IEnumerable<Capability> capabilities,
            IEnumerable<Industry> industries)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
            _serviceTypes = new List<ServiceType>(serviceTypes);
            _capabilities = new List<Capability>(capabilities);
            _industries = new List<Industry>(industries);
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Oppertunity()
        {
            Title = null!;
            ShortDescription = null!;
            FullDescription = null!;
            OppertunityType = null!;
            Country = null!;
            CompanyProfile = null!;
        }

        public string Title { get; private set; }

        public string ShortDescription { get; private set; }

        public string FullDescription { get; private set; }

        public DateOnly? Deadline { get; private set; }

        public Guid OppertunityTypeId { get; private set; }

        public Guid CompanyProfileId { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid? UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedDate { get; private set; }

        public Guid CountryId { get; private set; }

        public OppertunityStatus Status { get; private set; }

        public virtual OppertunityType OppertunityType { get; private set; }

        public virtual IReadOnlyCollection<ServiceType> ServiceTypes
        {
            get => _serviceTypes.AsReadOnly();
            private set => _serviceTypes = new List<ServiceType>(value);
        }

        public virtual IReadOnlyCollection<Capability> Capabilities
        {
            get => _capabilities.AsReadOnly();
            private set => _capabilities = new List<Capability>(value);
        }

        public virtual IReadOnlyCollection<Industry> Industries
        {
            get => _industries.AsReadOnly();
            private set => _industries = new List<Industry>(value);
        }

        public virtual Country Country { get; private set; }

        public virtual IReadOnlyCollection<CompanyProfile> InterestedPartners
        {
            get => _interestedPartners.AsReadOnly();
            private set => _interestedPartners = new List<CompanyProfile>(value);
        }

        public virtual CompanyProfile CompanyProfile { get; private set; }

        public void Update(
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
        }

        public void Update(
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId,
            IEnumerable<ServiceType> serviceTypes,
            IEnumerable<Capability> capabilities,
            IEnumerable<Industry> industries)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
            _serviceTypes.Clear();
            _serviceTypes.AddRange(serviceTypes);
            _capabilities.Clear();
            _capabilities.AddRange(capabilities);
            _industries.Clear();
            _industries.AddRange(industries);
        }

        public Task UpdateInterestedPartners(IEnumerable<Guid> companyProfileIds)
        {
            // TODO: Implement UpdateInterestedPartners (Oppertunity) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public Task UpdateIndustries(IEnumerable<Guid> industryIds)
        {
            // TODO: Implement UpdateIndustries (Oppertunity) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void UpdateCapabilities(IEnumerable<Guid> capabilityIds)
        {
            // TODO: Implement UpdateCapabilities (Oppertunity) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        public void UpdateServiceTypes(IEnumerable<Guid> serviceTypeIds)
        {
            // TODO: Implement UpdateServiceTypes (Oppertunity) functionality
            throw new NotImplementedException("Replace with your implementation...");
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}