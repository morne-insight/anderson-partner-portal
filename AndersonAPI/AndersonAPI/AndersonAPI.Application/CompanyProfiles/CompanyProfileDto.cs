using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles
{
    public record CompanyProfileDto
    {
        public CompanyProfileDto()
        {
            Name = null!;
            ShortDescription = null!;
            Description = null!;
            WebsiteUrl = null!;
            Embedding = null!;
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public int EmployeeCount { get; set; }
        public string Embedding { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }

        public static CompanyProfileDto Create(
            string name,
            string shortDescription,
            string description,
            string websiteUrl,
            int employeeCount,
            string embedding,
            Guid id,
            int order)
        {
            return new CompanyProfileDto
            {
                Name = name,
                ShortDescription = shortDescription,
                Description = description,
                WebsiteUrl = websiteUrl,
                EmployeeCount = employeeCount,
                Embedding = embedding,
                Id = id,
                Order = order
            };
        }
    }
}