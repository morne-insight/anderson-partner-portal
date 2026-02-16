using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public record DashboardServiceTypeDto
    {
        public DashboardServiceTypeDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static DashboardServiceTypeDto Create(Guid id, string name)
        {
            return new DashboardServiceTypeDto
            {
                Id = id,
                Name = name
            };
        }
    }
}