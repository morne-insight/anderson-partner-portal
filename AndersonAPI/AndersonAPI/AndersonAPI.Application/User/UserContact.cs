using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.User
{
    public record UserContact
    {
        public UserContact()
        {
            FirstName = null!;
            LastName = null!;
            Name = null!;
            WebsiteUrl = null!;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? CompanyPosition { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public Guid CompanyId { get; set; }

        public static UserContact Create(
            string firstName,
            string lastName,
            string? emailAddress,
            string? companyPosition,
            string name,
            string websiteUrl,
            Guid companyId)
        {
            return new UserContact
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                CompanyPosition = companyPosition,
                Name = name,
                WebsiteUrl = websiteUrl,
                CompanyId = companyId
            };
        }
    }
}