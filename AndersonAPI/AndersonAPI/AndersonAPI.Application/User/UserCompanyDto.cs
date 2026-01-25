using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.User
{
    public record UserCompanyDto
    {
        public UserCompanyDto()
        {
            Name = null!;
            WebsiteUrl = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }

        public static UserCompanyDto Create(Guid id, string name, string websiteUrl)
        {
            return new UserCompanyDto
            {
                Id = id,
                Name = name,
                WebsiteUrl = websiteUrl
            };
        }
    }
}