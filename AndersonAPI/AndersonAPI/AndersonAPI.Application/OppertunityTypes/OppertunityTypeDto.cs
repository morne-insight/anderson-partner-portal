using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.OppertunityTypes
{
    public record OppertunityTypeDto
    {
        public OppertunityTypeDto()
        {
            Name = null!;
            Description = null!;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public EntityState State { get; set; }

        public static OppertunityTypeDto Create(string name, string description, Guid id, int order, EntityState state)
        {
            return new OppertunityTypeDto
            {
                Name = name,
                Description = description,
                Id = id,
                Order = order,
                State = state
            };
        }
    }
}