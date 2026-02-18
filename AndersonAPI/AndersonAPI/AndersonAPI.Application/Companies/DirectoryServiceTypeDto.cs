using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record DirectoryServiceTypeDto
    {
        public DirectoryServiceTypeDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static DirectoryServiceTypeDto Create(Guid id, string name)
        {
            return new DirectoryServiceTypeDto
            {
                Id = id,
                Name = name
            };
        }
    }
}