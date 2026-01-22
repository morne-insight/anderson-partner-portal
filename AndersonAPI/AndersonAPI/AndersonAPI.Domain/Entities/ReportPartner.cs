using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class ReportPartner : BaseEntityList
    {
        public ReportPartner(string name,
            PartnerStatus status,
            EntityState state = EntityState.Enabled,
            string description = "")
        {
            Name = name;
            Status = status;
            State = state;
            Description = description;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected ReportPartner()
        {
        }

        public Guid QuaterlyId { get; private set; }

        public PartnerStatus Status { get; private set; }

        public void Update(string name, PartnerStatus status)
        {
            Name = name;
            Status = status;
        }
    }
}