using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyIndustryDto
    {
        public CompanyIndustryDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static CompanyIndustryDto Create(Guid id, string name)
        {
            return new CompanyIndustryDto
            {
                Id = id,
                Name = name
            };
        }
    }
}