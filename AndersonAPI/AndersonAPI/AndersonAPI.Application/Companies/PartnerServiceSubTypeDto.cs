using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerServiceSubTypeDto
    {
        public PartnerServiceSubTypeDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string Name { get; set; }

        public static PartnerServiceSubTypeDto Create(Guid id, Guid serviceTypeId, string name)
        {
            return new PartnerServiceSubTypeDto
            {
                Id = id,
                ServiceTypeId = serviceTypeId,
                Name = name
            };
        }
    }
}