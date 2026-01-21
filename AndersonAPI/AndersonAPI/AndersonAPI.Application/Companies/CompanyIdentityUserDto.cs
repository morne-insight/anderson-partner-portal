using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyIdentityUserDto
    {
        public CompanyIdentityUserDto()
        {
            Email = null!;
            UserName = null!;
        }

        public Guid Id { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public static CompanyIdentityUserDto Create(Guid id, string email, string userName)
        {
            return new CompanyIdentityUserDto
            {
                Id = id,
                Email = email,
                UserName = userName
            };
        }
    }
}