using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record DirectoryProfileListItem
    {
        public DirectoryProfileListItem()
        {
            Name = null!;
            ShortDescription = null!;
            Capabilities = null!;
            Locations = null!;
            Contacts = null!;
            Industries = null!;
            ServiceTypes = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public List<PartnerCapabilityDto> Capabilities { get; set; }
        public List<PartnerLocationDto> Locations { get; set; }
        public List<PartnerContactDto> Contacts { get; set; }
        public List<PartnerIndustryDto> Industries { get; set; }
        public List<PartnerServiceTypeDto> ServiceTypes { get; set; }

        public static DirectoryProfileListItem Create(
            Guid id,
            string name,
            string shortDescription,
            List<PartnerCapabilityDto> capabilities,
            List<PartnerLocationDto> locations,
            List<PartnerContactDto> contacts,
            List<PartnerIndustryDto> industries,
            List<PartnerServiceTypeDto> serviceTypes)
        {
            return new DirectoryProfileListItem
            {
                Id = id,
                Name = name,
                ShortDescription = shortDescription,
                Capabilities = capabilities,
                Locations = locations,
                Contacts = contacts,
                Industries = industries,
                ServiceTypes = serviceTypes
            };
        }
    }
}