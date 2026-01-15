using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public abstract class BaseEntityList : BaseEntity
    {
        public BaseEntityList()
        {
            Name = null!;
            Description = null!;
        }

        public string Name { get; protected set; }

        public string Description { get; protected set; }
    }
}