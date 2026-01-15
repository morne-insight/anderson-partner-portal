using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles
{
    public record CompanyNameDto
    {
        public CompanyNameDto()
        {
            Name = null!;
            ShortDescription = null!;
            WebsiteUrl = null!;
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string WebsiteUrl { get; set; }

        public static CompanyNameDto Create(string name, string shortDescription, string websiteUrl)
        {
            return new CompanyNameDto
            {
                Name = name,
                ShortDescription = shortDescription,
                WebsiteUrl = websiteUrl
            };
        }
    }
}