using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityServiceTypeDto
    {
        public OpportunityServiceTypeDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static OpportunityServiceTypeDto Create(Guid id, string name)
        {
            return new OpportunityServiceTypeDto
            {
                Id = id,
                Name = name
            };
        }
    }
}