using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public record CreateQuarterlyPartnersDto
    {
        public CreateQuarterlyPartnersDto()
        {
            Name = null!;
            Description = null!;
        }

        public Guid QuaterlyId { get; set; }
        public PartnerStatus Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public EntityState State { get; set; }

        public static CreateQuarterlyPartnersDto Create(
            Guid quaterlyId,
            PartnerStatus status,
            string name,
            string description,
            int order,
            EntityState state)
        {
            return new CreateQuarterlyPartnersDto
            {
                QuaterlyId = quaterlyId,
                Status = status,
                Name = name,
                Description = description,
                Order = order,
                State = state
            };
        }
    }
}