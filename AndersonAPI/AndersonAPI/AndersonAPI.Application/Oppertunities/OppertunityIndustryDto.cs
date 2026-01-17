using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public record OppertunityIndustryDto
    {
        public OppertunityIndustryDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static OppertunityIndustryDto Create(Guid id, string name)
        {
            return new OppertunityIndustryDto
            {
                Id = id,
                Name = name
            };
        }
    }
}