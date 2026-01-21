using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class ReportPartner : BaseEntityList
    {
        public ReportPartner(Guid quaterlyId, string name, PartnerStatus status, EntityState state = EntityState.Enabled)
        {
            QuaterlyId = quaterlyId;
            Name = name;
            Status = status;
            State = state;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected ReportPartner()
        {
        }

        public Guid QuaterlyId { get; private set; }

        public PartnerStatus Status { get; private set; }

        public void Update(string name)
        {
            Name = name;
        }
    }
}