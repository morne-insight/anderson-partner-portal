using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityCapabilityDto
    {
        public OpportunityCapabilityDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static OpportunityCapabilityDto Create(Guid id, string name)
        {
            return new OpportunityCapabilityDto
            {
                Id = id,
                Name = name
            };
        }
    }
}