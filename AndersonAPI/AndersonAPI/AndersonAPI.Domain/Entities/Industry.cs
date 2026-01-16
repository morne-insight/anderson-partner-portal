using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Industry : BaseEntityList
    {
        public Industry(string name, string description = "", EntityState state = EntityState.Enabled)
        {
            Name = name;
            Description = description;
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Industry()
        {
        }

        public void Update(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}