using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerIndustryDto
    {
        public PartnerIndustryDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static PartnerIndustryDto Create(Guid id, string name)
        {
            return new PartnerIndustryDto
            {
                Id = id,
                Name = name
            };
        }
    }
}