using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyDto
    {
        public CompanyDto()
        {
            Name = null!;
            ShortDescription = null!;
            FullDescription = null!;
            WebsiteUrl = null!;
            ServiceTypeName = null!;
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int EmployeeCount { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string ServiceTypeName { get; set; }

        public static CompanyDto Create(
            Guid id,
            string name,
            string shortDescription,
            string fullDescription,
            string websiteUrl,
            int employeeCount,
            int order,
            string serviceTypeName)
        {
            return new CompanyDto
            {
                Id = id,
                Name = name,
                ShortDescription = shortDescription,
                FullDescription = fullDescription,
                WebsiteUrl = websiteUrl,
                EmployeeCount = employeeCount,
                Order = order,
                ServiceTypeName = serviceTypeName
            };
        }
    }
}