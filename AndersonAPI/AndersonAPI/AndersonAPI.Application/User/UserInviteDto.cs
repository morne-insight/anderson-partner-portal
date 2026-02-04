using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.User
{
    public record UserInviteDto
    {
        public UserInviteDto()
        {
            CompanyName = null!;
            CompanyShortDescription = null!;
            CompanyWebsiteUrl = null!;
        }

        public string CompanyName { get; set; }
        public string CompanyShortDescription { get; set; }
        public string CompanyWebsiteUrl { get; set; }
        public Guid Id { get; set; }

        public static UserInviteDto Create(
            string companyName,
            string companyShortDescription,
            string companyWebsiteUrl,
            Guid id)
        {
            return new UserInviteDto
            {
                CompanyName = companyName,
                CompanyShortDescription = companyShortDescription,
                CompanyWebsiteUrl = companyWebsiteUrl,
                Id = id
            };
        }
    }
}