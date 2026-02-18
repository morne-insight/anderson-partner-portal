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
            ServiceTypes = null!;
            ServiceSubTypes = null!;
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
        public double MatchScore { get; set; }
        public List<PartnerServiceSubTypeDto> ServiceSubTypes { get; set; }
        public List<PartnerServiceTypeDto> ServiceTypes { get; set; }

        public static PartnerProfileListItem Create(
            Guid id,
            string name,
            string shortDescription,
            List<PartnerServiceTypeDto> serviceTypes,
            List<PartnerServiceSubTypeDto> serviceSubTypes,
            List<PartnerCapabilityDto> capabilities,
            List<PartnerLocationDto> locations,
            List<PartnerContactDto> contacts,
            double matchScore)
        {
            return new PartnerProfileListItem
            {
                Id = id,
                Name = name,
                ShortDescription = shortDescription,
                ServiceTypes = serviceTypes,
                ServiceSubTypes = serviceSubTypes
,
                Capabilities = capabilities,
                Locations = locations,
                Contacts = contacts,
                MatchScore = matchScore
            };
        }
    }
}