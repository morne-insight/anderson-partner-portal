using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public record OppertunityServiceTypeDto
    {
        public OppertunityServiceTypeDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static OppertunityServiceTypeDto Create(Guid id, string name)
        {
            return new OppertunityServiceTypeDto
            {
                Id = id,
                Name = name
            };
        }
    }
}