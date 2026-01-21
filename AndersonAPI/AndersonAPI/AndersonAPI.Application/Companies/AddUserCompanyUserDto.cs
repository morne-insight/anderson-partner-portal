using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record AddUserCompanyUserDto
    {
        public AddUserCompanyUserDto()
        {
        }

        public static AddUserCompanyUserDto Create()
        {
            return new AddUserCompanyUserDto
            {
            };
        }
    }
}