using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class OppertunityType : BaseEntityList
    {
        public OppertunityType(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected OppertunityType()
        {
        }

        public void Update(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}