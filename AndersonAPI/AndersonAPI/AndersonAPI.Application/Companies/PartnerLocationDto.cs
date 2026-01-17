using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerLocationDto
    {
        public PartnerLocationDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsHeadOffice { get; set; }
        public Guid RegionId { get; set; }
        public Guid CountryId { get; set; }

        public static PartnerLocationDto Create(Guid id, string name, bool isHeadOffice, Guid regionId, Guid countryId)
        {
            return new PartnerLocationDto
            {
                Id = id,
                Name = name,
                IsHeadOffice = isHeadOffice,
                RegionId = regionId,
                CountryId = countryId
            };
        }
    }
}