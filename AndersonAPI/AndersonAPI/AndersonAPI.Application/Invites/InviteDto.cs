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
        }

        public string Email { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }

        public static InviteDto Create(Guid id, Guid companyId, string email, string name)
        {
            return new InviteDto
            {
                Id = id,
                CompanyId = companyId,
                Email = email,
                Name = name
            };
        }
    }
}