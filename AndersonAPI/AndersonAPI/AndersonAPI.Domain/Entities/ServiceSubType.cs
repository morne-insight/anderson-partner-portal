using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class ServiceSubType : BaseEntityList
    {
        public ServiceSubType(Guid serviceTypeId,
            string name,
            string description = "",
            EntityState state = EntityState.Enabled)
        {
            ServiceTypeId = serviceTypeId;
            Name = name;
            Description = description;
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected ServiceSubType()
        {
            ServiceType = null!;
        }

        public Guid ServiceTypeId { get; private set; }

        public virtual ServiceType ServiceType { get; private set; }


        public void Update(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}