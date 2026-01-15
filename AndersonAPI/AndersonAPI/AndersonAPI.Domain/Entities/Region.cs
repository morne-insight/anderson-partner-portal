using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Region : BaseEntityList
    {
        private List<Country> _countries = [];

        public Region(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Region()
        {
        }

        public virtual IReadOnlyCollection<Country> Countries
        {
            get => _countries.AsReadOnly();
            private set => _countries = new List<Country>(value);
        }

        public void Update(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}