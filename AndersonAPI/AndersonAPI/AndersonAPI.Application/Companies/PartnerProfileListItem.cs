using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerProfileListItem
    {
        public PartnerProfileListItem()
        {
            Name = null!;
            ShortDescription = null!;
            Capabilities = null!;
            Locations = null!;
            Contacts = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public List<PartnerCapabilityDto> Capabilities { get; set; }
        public List<PartnerLocationDto> Locations { get; set; }
        public List<PartnerContactDto> Contacts { get; set; }
        public int MatchScore { get; set; }

        public static PartnerProfileListItem Create(
            Guid id,
            string name,
            string shortDescription,
            List<PartnerCapabilityDto> capabilities,
            List<PartnerLocationDto> locations,
            List<PartnerContactDto> contacts,
            int matchScore)
        {
            return new PartnerProfileListItem
            {
                Id = id,
                Name = name,
                ShortDescription = shortDescription,
                Capabilities = capabilities,
                Locations = locations,
                Contacts = contacts,
                MatchScore = matchScore
            };
        }
    }
}