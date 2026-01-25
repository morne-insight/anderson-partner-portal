using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityViewPartnerDto
    {
        public OpportunityViewPartnerDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static OpportunityViewPartnerDto Create(Guid id, string name)
        {
            return new OpportunityViewPartnerDto
            {
                Id = id,
                Name = name
            };
        }
    }
}