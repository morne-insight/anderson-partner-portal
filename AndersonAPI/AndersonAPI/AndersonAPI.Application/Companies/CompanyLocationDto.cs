using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyLocationDto
    {
        public CompanyLocationDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsHeadOffice { get; set; }
        public Guid RegionId { get; set; }
        public Guid CountryId { get; set; }
        public int Order { get; set; }

        public static CompanyLocationDto Create(Guid id, string name, bool isHeadOffice, Guid regionId, Guid countryId, int order)
        {
            return new CompanyLocationDto
            {
                Id = id,
                Name = name,
                IsHeadOffice = isHeadOffice,
                RegionId = regionId,
                CountryId = countryId,
                Order = order
            };
        }
    }
}