using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies
{
    public record QuarterlyDto
    {
        public QuarterlyDto()
        {
        }

        public int Quarter { get; set; }
        public bool IsSubmitted { get; set; }
        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public EntityState State { get; set; }

        public static QuarterlyDto Create(int quarter, bool isSubmitted, Guid companyId, Guid id, int order, EntityState state)
        {
            return new QuarterlyDto
            {
                Quarter = quarter,
                IsSubmitted = isSubmitted,
                CompanyId = companyId,
                Id = id,
                Order = order,
                State = state
            };
        }
    }
}