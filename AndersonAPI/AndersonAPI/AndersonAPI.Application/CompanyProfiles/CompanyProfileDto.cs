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
            FullDescription = null!;
            WebsiteUrl = null!;
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int EmployeeCount { get; set; }
        public byte[]? Embedding { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }

        public static CompanyProfileDto Create(
            string name,
            string shortDescription,
            string fullDescription,
            string websiteUrl,
            int employeeCount,
            byte[]? embedding,
            Guid id,
            int order)
        {
            return new CompanyProfileDto
            {
                Name = name,
                ShortDescription = shortDescription,
                FullDescription = fullDescription,
                WebsiteUrl = websiteUrl,
                EmployeeCount = employeeCount,
                Embedding = embedding,
                Id = id,
                Order = order
            };
        }
    }
}