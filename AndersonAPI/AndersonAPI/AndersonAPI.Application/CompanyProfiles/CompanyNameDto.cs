using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles
{
    public record CompanyNameDto
    {
        public CompanyNameDto()
        {
            Name = null!;
        }

        public string Name { get; set; }

        public static CompanyNameDto Create(string name)
        {
            return new CompanyNameDto
            {
                Name = name
            };
        }
    }
}