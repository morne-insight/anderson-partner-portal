using System.Globalization;
using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Industry : BaseEntityList
    {
        private readonly TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public Industry(string name, string description = "", EntityState state = EntityState.Enabled)
        {
            var formattedName = textInfo.ToTitleCase(name.ToLower());

            Name = formattedName;
            Description = description;
            State = state;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Industry()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public void Update(string name, string description = "")
        {
            var formattedName = textInfo.ToTitleCase(name.ToLower());

            Name = formattedName;
            Description = description;
        }
    }
}