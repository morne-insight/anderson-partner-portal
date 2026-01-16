using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Country : BaseEntityList
    {
        public Country(Guid? regionId, string name, string description = "", EntityState state = EntityState.Enabled)
        {
            RegionId = regionId;
            Name = name;
            Description = description;
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Country()
        {
        }

        public Guid? RegionId { get; private set; }

        public void Update(Guid? regionId, string name, string description = "")
        {
            RegionId = regionId;
            Name = name;
            Description = description;
        }
    }
}