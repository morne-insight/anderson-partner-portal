using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public record OppertunityCapabilityDto
    {
        public OppertunityCapabilityDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static OppertunityCapabilityDto Create(Guid id, string name)
        {
            return new OppertunityCapabilityDto
            {
                Id = id,
                Name = name
            };
        }
    }
}