using AndersonAPI.Domain.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public abstract class BaseEntity : IHasDomainEvent
    {
        public Guid Id { get; protected set; }

        public int Order { get; protected set; } = 0;

        public EntityState State { get; protected set; }

        public List<DomainEvent> DomainEvents { get; set; } = [];
    }
}