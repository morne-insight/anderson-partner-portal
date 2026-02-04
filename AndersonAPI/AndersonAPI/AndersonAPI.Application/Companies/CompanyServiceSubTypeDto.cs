using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyServiceSubTypeDto
    {
        public CompanyServiceSubTypeDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string Name { get; set; }

        public static CompanyServiceSubTypeDto Create(Guid id, Guid serviceTypeId, string name)
        {
            return new CompanyServiceSubTypeDto
            {
                Id = id,
                ServiceTypeId = serviceTypeId,
                Name = name
            };
        }
    }
}