using AndersonAPI.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Opportunity : BaseEntity, IAuditable
    {
        private List<ServiceType> _serviceTypes = [];
        private List<Capability> _capabilities = [];
        private List<Industry> _industries = [];
        private List<Company> _interestedPartners = [];
        private List<Message> _messages = [];

        public Opportunity(string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid opportunityTypeId,
            Guid countryId,
            Guid companyId,
            EntityState state = EntityState.Enabled,
            OpportunityStatus status = OpportunityStatus.Open)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OpportunityTypeId = opportunityTypeId;
            CountryId = countryId;
            CompanyId = companyId;
            State = state;
            Status = status;
        }

        public Opportunity(string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid opportunityTypeId,
            Guid countryId,
            Guid companyId,
            IEnumerable<Guid> serviceTypes,
            IEnumerable<Guid> capabilities,
            IEnumerable<Guid> industries,
            EntityState state = EntityState.Enabled,
            OpportunityStatus status = OpportunityStatus.Open)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OpportunityTypeId = opportunityTypeId;
            CountryId = countryId;
            CompanyId = companyId;
            State = state;
            Status = status;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Opportunity()
        {
            Title = null!;
            ShortDescription = null!;
            FullDescription = null!;
            OpportunityType = null!;
            Company = null!;
            Country = null!;
        }

        public string Title { get; private set; }

        public string ShortDescription { get; private set; }

        public string FullDescription { get; private set; }

        public DateOnly? Deadline { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid? UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedDate { get; private set; }

        public Guid CompanyId { get; private set; }

        public virtual OpportunityType OpportunityType { get; private set; }

        public Guid CountryId { get; private set; }

        public OpportunityStatus Status { get; private set; }

        public Guid OpportunityTypeId { get; private set; }

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

        public void Update(
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid opportunityTypeId,
            Guid countryId)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OpportunityTypeId = opportunityTypeId;
            CountryId = countryId;
        }

        public void UpdateFull(
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid opportunityTypeId,
            Guid countryId,
            IEnumerable<Guid> serviceTypes,
            IEnumerable<Guid> capabilities,
            IEnumerable<Guid> industries)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OpportunityTypeId = opportunityTypeId;
            CountryId = countryId;
        }

        public void AddInterestedPartner(Company partner)
        {
            _interestedPartners.Add(partner);
        }

        public void RemoveInterestedPartner(Guid partnerId)
        {
            var partner = _interestedPartners.FirstOrDefault(p => p.Id == partnerId);
            if (partner != null)
            {
                _interestedPartners.Remove(partner);
            }
        }

        public virtual IReadOnlyCollection<Company> InterestedPartners
        {
            get => _interestedPartners.AsReadOnly();
            private set => _interestedPartners = new List<Company>(value);
        }

        public virtual Company Company { get; private set; }

        public virtual IReadOnlyCollection<Message> Messages
        {
            get => _messages.AsReadOnly();
            private set => _messages = new List<Message>(value);
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
            for (int i = _capabilities.Count - 1; i >= 0; i--)
            {
                if (!targetIds.Contains(_capabilities[i].Id))
                {
                    _capabilities.RemoveAt(i);
                }
            }

            // Add any new capabilities that are not already in the collection
            var currentIds = _capabilities.Select(p => p.Id).ToHashSet();
            var newCapabilities = targetList.Where(capability => !currentIds.Contains(capability.Id));

            foreach (var capability in newCapabilities)
            {
                _capabilities.Add(capability);
            }
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

        public void SetStatus(OpportunityStatus status)
        {
            Status = status;
        }

        public void AddMessage(
            Guid opportunityId,
            string content,
            DateTimeOffset createdDate,
            Guid createdByUserId,
            string createdByUser,
            string? createdByPartner,
            bool isOwnMessage)
        {
            var message = new Message(opportunityId, content, createdDate, createdByUserId, createdByUser, createdByPartner, isOwnMessage);
            _messages.Add(message);
        }

        public void UpdateMessage(Guid messageId, string content)
        {
            var message = _messages.FirstOrDefault(m => m.Id == messageId);

            if (message == null)
            {
                throw new InvalidOperationException($"Message with ID '{messageId}' not found.");
            }

            // Get the last message ordered by CreatedDate
            var lastMessage = _messages.OrderByDescending(m => m.CreatedDate).FirstOrDefault();

            if (lastMessage == null || lastMessage.Id != messageId)
            {
                throw new InvalidOperationException("Only the most recent message can be updated.");
            }

            message.UpdateContent(content);
        }

        public void RemoveMessage(Guid id)
        {
            var message = _messages.FirstOrDefault(m => m.Id == id);

            if (message == null)
            {
                throw new InvalidOperationException($"Message with ID '{id}' not found.");
            }

            // Get the last message ordered by CreatedDate
            var lastMessage = _messages.OrderByDescending(m => m.CreatedDate).FirstOrDefault();

            if (lastMessage == null || lastMessage.Id != id)
            {
                throw new InvalidOperationException("Only the most recent message can be updated.");
            }

            _messages.Remove(message);
        }

        void IAuditable.SetCreated(Guid createdBy, DateTimeOffset createdDate) => (CreatedBy, CreatedDate) = (createdBy, createdDate);

        void IAuditable.SetUpdated(Guid updatedBy, DateTimeOffset updatedDate) => (UpdatedBy, UpdatedDate) = (updatedBy, updatedDate);
    }
}