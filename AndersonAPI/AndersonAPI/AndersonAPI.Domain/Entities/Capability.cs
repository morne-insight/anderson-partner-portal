using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Capability : BaseEntityList
    {
        public Capability(string name, string description = "", EntityState state = EntityState.Enabled)
        {
            Name = name;
            Description = description;
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Capability()
        {
        }

        public void Update(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}