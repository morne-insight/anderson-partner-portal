using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyServiceTypeDto
    {
        public CompanyServiceTypeDto()
        {
            Name = null!;
            Description = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public EntityState State { get; set; }

        public static CompanyServiceTypeDto Create(Guid id, string name, string description, int order, EntityState state)
        {
            return new CompanyServiceTypeDto
            {
                Id = id,
                Name = name,
                Description = description,
                Order = order,
                State = state
            };
        }
    }
}