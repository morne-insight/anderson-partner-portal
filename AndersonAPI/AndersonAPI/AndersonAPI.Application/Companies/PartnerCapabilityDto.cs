using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerCapabilityDto
    {
        public PartnerCapabilityDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static PartnerCapabilityDto Create(Guid id, string name)
        {
            return new PartnerCapabilityDto
            {
                Id = id,
                Name = name
            };
        }
    }
}