using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityIndustryDto
    {
        public OpportunityIndustryDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static OpportunityIndustryDto Create(Guid id, string name)
        {
            return new OpportunityIndustryDto
            {
                Id = id,
                Name = name
            };
        }
    }
}