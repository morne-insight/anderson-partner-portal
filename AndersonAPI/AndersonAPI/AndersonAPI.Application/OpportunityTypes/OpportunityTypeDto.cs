using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.OpportunityTypes
{
    public record OpportunityTypeDto
    {
        public OpportunityTypeDto()
        {
            Name = null!;
            Description = null!;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public EntityState State { get; set; }

        public static OpportunityTypeDto Create(string name, string description, Guid id, int order, EntityState state)
        {
            return new OpportunityTypeDto
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