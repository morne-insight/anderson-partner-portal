using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Invites
{
    public record InviteDto
    {
        public InviteDto()
        {
            Email = null!;
            Name = null!;
            CompanyName = null!;
            CompanyShortDescription = null!;
            CompanyWebsiteUrl = null!;
        }

        public string Email { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortDescription { get; set; }
        public string CompanyWebsiteUrl { get; set; }
        public Guid Id { get; set; }

        public static InviteDto Create(
            Guid id,
            Guid companyId,
            string email,
            string name,
            string companyName,
            string companyShortDescription,
            string companyWebsiteUrl)
        {
            return new InviteDto
            {
                Id = id,
                CompanyId = companyId,
                Email = email,
                Name = name,
                CompanyName = companyName,
                CompanyShortDescription = companyShortDescription,
                CompanyWebsiteUrl = companyWebsiteUrl
            };
        }
    }
}