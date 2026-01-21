using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record RemoveUserCompanyUserDto
    {
        public RemoveUserCompanyUserDto()
        {
        }

        public static RemoveUserCompanyUserDto Create()
        {
            return new RemoveUserCompanyUserDto
            {
            };
        }
    }
}