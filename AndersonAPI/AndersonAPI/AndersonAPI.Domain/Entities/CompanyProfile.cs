using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class CompanyProfile : EntityBase
    {
        public CompanyProfile(string name, string shortDescription, string description, string websiteUrl, int employeeCount)
        {
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected CompanyProfile()
        {
            Name = null!;
            ShortDescription = null!;
            Description = null!;
            WebsiteUrl = null!;
            Embedding = null!;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string ShortDescription { get; private set; }

        public string WebsiteUrl { get; private set; }

        public int EmployeeCount { get; private set; }

        public string Embedding { get; private set; }

        public void UpdateProfile(string name, string shortDescription, string description, string websiteUrl, int employeeCount)
        {
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
        }
    }
}