using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Invite : BaseEntityList
    {
        public Invite(string email, Guid companyId, string description = "", string name = "")
        {
            Email = email;
            CompanyId = companyId;
            Description = description;
            Name = name;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Invite()
        {
            Email = null!;
            Company = null!;
        }

        public string Email { get; private set; }

        public Guid CompanyId { get; private set; }

        public virtual Company Company { get; private set; }

        public void Update(string email)
        {
            Email = email;
        }
    }
}