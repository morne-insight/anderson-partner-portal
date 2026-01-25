using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.User
{
    public record UserDetailDto
    {
        public UserDetailDto()
        {
            Name = null!;
            Companies = null!;
        }

        public string Name { get; set; }
        public Guid? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public List<UserCompanyDto> Companies { get; set; }

        public static UserDetailDto Create(string name, Guid? companyId, string? companyName, List<UserCompanyDto> companies)
        {
            return new UserDetailDto
            {
                Name = name,
                CompanyId = companyId,
                CompanyName = companyName,
                Companies = companies
            };
        }
    }
}