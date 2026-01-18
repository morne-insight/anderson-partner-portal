using AndersonAPI.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Company : BaseEntity, IAuditable
    {
        private List<Location> _locations = [];
        private List<Industry> _industries = [];
        private List<Capability> _capabilities = [];
        private List<ServiceType> _serviceTypes = [];
        private List<Contact> _contacts = [];
        private List<ApplicationIdentityUser> _applicationIdentityUsers = [];
        private List<Invite> _invites = [];
        private List<Review> _reviews = [];
        private List<Opportunity> _opportunities = [];
        private List<Opportunity> _savedOpportunities = [];
        public Company(string name,
            string shortDescription,
            string fullDescription,
            string websiteUrl,
            int employeeCount,
            IEnumerable<Industry> industries,
            IEnumerable<Capability> capabilities,
            IEnumerable<ServiceType> serviceTypes,
            EntityState state = EntityState.Enabled)
        {
            Name = name;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
            _industries = new List<Industry>(industries);
            _capabilities = new List<Capability>(capabilities);
            _serviceTypes = new List<ServiceType>(serviceTypes);
            State = state;
        }

        public Company(string name,
            string shortDescription,
            string fullDescription,
            string websiteUrl,
            int employeeCount,
            EntityState state = EntityState.Enabled)
        {
            Name = name;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
            State = state;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Company()
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

        public virtual IReadOnlyCollection<Invite> Invites
        {
            get => _invites.AsReadOnly();
            private set => _invites = new List<Invite>(value);
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

        public virtual IReadOnlyCollection<Opportunity>? Opportunities
        {
            get => _opportunities.AsReadOnly();
            private set => _opportunities = new List<Opportunity>(value);
        }

        public virtual IReadOnlyCollection<Opportunity>? SavedOpportunities
        {
            get => _savedOpportunities.AsReadOnly();
            private set => _savedOpportunities = new List<Opportunity>(value);
        }

        public void AddLocation(string name, Guid regionId, Guid countryId, bool isHeadOffice)
        {
            var location = new Location(name, regionId, countryId, isHeadOffice, Id);

            if (isHeadOffice)
            {
                // If this location is marked as head office, ensure no other location is a head office
                foreach (var existingLocation in _locations.Where(l => l.IsHeadOffice))
                {
                    existingLocation.SetHeadOffice(false);
                }
            }

            _locations.Add(location);
        }

        public void UpdateLocation(Guid locationId, string name, Guid regionId, Guid countryId, bool isHeadOffice)
        {
            var location = _locations.FirstOrDefault(l => l.Id == locationId);
            if (location == null)
            {
                throw new InvalidOperationException($"Location with ID {locationId} not found.");
            }

            if (isHeadOffice && !location.IsHeadOffice)
            {
                // If this location is being set as head office, ensure no other location is a head office
                foreach (var existingLocation in _locations.Where(l => l.IsHeadOffice))
                {
                    existingLocation.SetHeadOffice(false);
                }
                location.SetHeadOffice(true);
            }
            else if (!isHeadOffice && location.IsHeadOffice)
            {
                location.SetHeadOffice(false);
            }

            location.Update(name, regionId, countryId, isHeadOffice);
        }

        public void RemoveLocation(Guid locationId)
        {
            var location = _locations.FirstOrDefault(l => l.Id == locationId);
            if (location == null)
            {
                throw new InvalidOperationException($"Location with ID {locationId} not found.");
            }

            _locations.Remove(location);
        }

        public void SetHeadOffice(Guid locationId)
        {
            var location = _locations.FirstOrDefault(l => l.Id == locationId);
            if (location == null)
            {
                throw new InvalidOperationException($"Location with ID {locationId} not found.");
            }

            // If this location is being set as head office, ensure no other location is a head office
            foreach (var existingLocation in _locations.Where(l => l.IsHeadOffice))
            {
                existingLocation.SetHeadOffice(false);
            }

            location.SetHeadOffice(true);
        }

        public void AddContact(string firstName, string lastName, string? emailAddress, string? companyPosition)
        {
            var contact = new Contact(firstName, lastName, emailAddress, companyPosition, Id);
            _contacts.Add(contact);
        }

        public void UpdateContact(
            Guid contactId,
            string firstName,
            string lastName,
            string? emailAddress,
            string? companyPosition)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == contactId);
            if (contact == null)
            {
                throw new InvalidOperationException($"Contact with ID {contactId} not found.");
            }

            contact.Update(firstName, lastName, emailAddress, companyPosition);
        }

        public void RemoveContact(Guid contactId)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == contactId);
            if (contact == null)
            {
                throw new InvalidOperationException($"Contact with ID {contactId} not found.");
            }

            _contacts.Remove(contact);
        }

        public void SetIndustries(IEnumerable<Industry> industries)
        {
            _industries.Clear();
            foreach (var industry in industries)
            {
                _industries.Add(industry);
            }
        }

        public void SetCapabilities(IEnumerable<Capability> capabilities)
        {
            _capabilities.Clear();
            foreach (var capability in capabilities)
            {
                _capabilities.Add(capability);
            }
        }

        public void SetServiceTypes(IEnumerable<ServiceType> serviceTypes)
        {
            _serviceTypes.Clear();
            foreach (var serviceType in serviceTypes)
            {
                _serviceTypes.Add(serviceType);
            }
        }

        public void AddUser(ApplicationIdentityUser user)
        {
            _applicationIdentityUsers.Add(user);
        }

        public void RemoveUser(ApplicationIdentityUser user)
        {
            _applicationIdentityUsers.RemoveAll(u => u.Id == user.Id);
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}