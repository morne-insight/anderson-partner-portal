using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyInviteDto
    {
        public CompanyInviteDto()
        {
            Name = null!;
            Email = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public static CompanyInviteDto Create(Guid id, string name, string email)
        {
            return new CompanyInviteDto
            {
                Id = id,
                Name = name,
                Email = email
            };
        }
    }
}