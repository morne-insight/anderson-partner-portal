using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Industries
{
    public record IndustryDto
    {
        public IndustryDto()
        {
            Name = null!;
            Description = null!;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }

        public static IndustryDto Create(string name, string description, Guid id, int order)
        {
            return new IndustryDto
            {
                Name = name,
                Description = description,
                Id = id,
                Order = order
            };
        }
    }
}