using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyApplicationIdentityUserDto
    {
        public CompanyApplicationIdentityUserDto()
        {
        }

        public static CompanyApplicationIdentityUserDto Create()
        {
            return new CompanyApplicationIdentityUserDto
            {
            };
        }
    }
}