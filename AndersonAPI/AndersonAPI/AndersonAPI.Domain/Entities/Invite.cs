using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Invite : BaseEntityList
    {
        public Invite()
        {
            Email = null!;
            Company = null!;
        }

        public string Email { get; private set; }

        public Guid CompanyId { get; private set; }

        public virtual Company Company { get; private set; }
    }
}