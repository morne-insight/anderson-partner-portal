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
            ServiceTypes = null!;
            ServiceSubTypes = null!;
            Capabilities = null!;
            Locations = null!;
            Contacts = null!;
            Industries = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public List<DirectoryServiceTypeDto> ServiceTypes { get; set; }
        public List<PartnerCapabilityDto> Capabilities { get; set; }
        public List<PartnerLocationDto> Locations { get; set; }
        public List<PartnerContactDto> Contacts { get; set; }
        public List<PartnerIndustryDto> Industries { get; set; }
        public List<PartnerServiceSubTypeDto> ServiceSubTypes { get; set; }

        public static DirectoryProfileListItem Create(
            Guid id,
            string name,
            string shortDescription,
            List<DirectoryServiceTypeDto> serviceTypes,
            List<PartnerServiceSubTypeDto> serviceSubTypes,
            List<PartnerCapabilityDto> capabilities,
            List<PartnerLocationDto> locations,
            List<PartnerContactDto> contacts,
            List<PartnerIndustryDto> industries)
        {
            return new DirectoryProfileListItem
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
                Industries = industries
            };
        }
    }
}