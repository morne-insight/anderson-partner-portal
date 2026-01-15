using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Capabilities
{
    public record CapabilityDto
    {
        public CapabilityDto()
        {
            Name = null!;
            Description = null!;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }

        public static CapabilityDto Create(string name, string description, Guid id, int order)
        {
            return new CapabilityDto
            {
                Name = name,
                Description = description,
                Id = id,
                Order = order
            };
        }
    }
}