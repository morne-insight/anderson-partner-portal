using Intent.RoslynWeaver.Attributes;
using System.Globalization;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Capability : BaseEntityList
    {
        private readonly TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

        public Capability(string name, string description = "", EntityState state = EntityState.Enabled)
        {
            var formattedName = textInfo.ToTitleCase(name.ToLower());

            Name = formattedName;
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
            var formattedName = textInfo.ToTitleCase(name.ToLower());

            Name = formattedName;
            Description = description;
        }
    }
}