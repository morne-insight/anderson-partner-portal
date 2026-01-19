using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanySearchDto
    {
        public CompanySearchDto()
        {
            Name = null!;
        }

        public string Name { get; set; }
        public byte[]? Embedding { get; set; }
        public double Score { get; set; }
        public Guid Id { get; set; }

        public static CompanySearchDto Create(Guid id, string name, byte[]? embedding, double score)
        {
            return new CompanySearchDto
            {
                Id = id
,
                Name = name,
                Embedding = embedding,
                Score = score
            };
        }
    }
}