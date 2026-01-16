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
        private List<Company> _interestedPartners = [];

        public Oppertunity(string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId,
            Guid companyId,
            EntityState state = EntityState.Enabled)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
            CompanyId = companyId;
            State = state;
        }

        public Oppertunity(string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId,
            Guid companyId,
            IEnumerable<Guid> serviceTypes,
            IEnumerable<Guid> capabilities,
            IEnumerable<Guid> industries,
            EntityState state = EntityState.Enabled)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
            CompanyId = companyId;
            State = state;
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
            Company = null!;
        }

        public string Title { get; private set; }

        public string ShortDescription { get; private set; }

        public string FullDescription { get; private set; }

        public DateOnly? Deadline { get; private set; }

        public Guid OppertunityTypeId { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid? UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedDate { get; private set; }

        public Guid CompanyId { get; private set; }

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

        public virtual IReadOnlyCollection<Company> InterestedPartners
        {
            get => _interestedPartners.AsReadOnly();
            private set => _interestedPartners = new List<Company>(value);
        }

        public virtual Company Company { get; private set; }

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

        public void UpdateFull(
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId,
            IEnumerable<Guid> serviceTypes,
            IEnumerable<Guid> capabilities,
            IEnumerable<Guid> industries)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
        }

        public void SetInterestedPartners(IEnumerable<Company> companies)
        {
            if (companies == null) throw new ArgumentNullException(nameof(companies));

            var targetList = companies.DistinctBy(cp => cp.Id).ToList();
            var targetIds = targetList.Select(cp => cp.Id).ToHashSet();

            // Remove any partners that are no longer in the new collection
            _interestedPartners.RemoveAll(p => !targetIds.Contains(p.Id));

            // Add any new partners that are not already in the collection
            var currentIds = _interestedPartners.Select(p => p.Id).ToHashSet();
            var newPartners = targetList.Where(partner => !currentIds.Contains(partner.Id));

            _interestedPartners.AddRange(newPartners);
        }

        public void SetIndustries(IEnumerable<Industry> industries)
        {
            if (industries == null) throw new ArgumentNullException(nameof(industries));

            var targetList = industries.DistinctBy(i => i.Id).ToList();
            var targetIds = targetList.Select(i => i.Id).ToHashSet();

            // Remove any industries that are no longer in the new collection
            _industries.RemoveAll(p => !targetIds.Contains(p.Id));

            // Add any new industries that are not already in the collection
            var currentIds = _industries.Select(p => p.Id).ToHashSet();
            var newIndustries = targetList.Where(industy => !currentIds.Contains(industy.Id));

            _industries.AddRange(newIndustries);
        }

        public void SetCapabilities(IEnumerable<Capability> capabilities)
        {
            if (capabilities == null) throw new ArgumentNullException(nameof(capabilities));

            var targetList = capabilities.DistinctBy(i => i.Id).ToList();
            var targetIds = targetList.Select(i => i.Id).ToHashSet();

            // Remove any capabilities that are no longer in the new collection
            _capabilities.RemoveAll(p => !targetIds.Contains(p.Id));

            // Add any new capabilities that are not already in the collection
            var currentIds = _capabilities.Select(p => p.Id).ToHashSet();
            var newCapabilities = targetList.Where(capability => !currentIds.Contains(capability.Id));
            _capabilities.AddRange(newCapabilities);
        }

        public void SetServiceTypes(IEnumerable<ServiceType> serviceTypes)
        {
            if (serviceTypes == null) throw new ArgumentNullException(nameof(serviceTypes));

            var targetList = serviceTypes.DistinctBy(i => i.Id).ToList();
            var targetIds = targetList.Select(i => i.Id).ToHashSet();

            // Remove any ServiceTypes that are no longer in the new collection
            _serviceTypes.RemoveAll(p => !targetIds.Contains(p.Id));

            // Add any new ServiceTypes that are not already in the collection
            var currentIds = _serviceTypes.Select(p => p.Id).ToHashSet();
            var newServiceTypes = targetList.Where(serviceType => !currentIds.Contains(serviceType.Id));

            _serviceTypes.AddRange(newServiceTypes);
        }

        public void SetStatus(OppertunityStatus status)
        {
            Status = status;
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}