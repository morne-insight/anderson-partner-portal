using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public abstract class EntityListBase : EntityBase
    {
        public EntityListBase()
        {
            Name = null!;
            Description = null!;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}