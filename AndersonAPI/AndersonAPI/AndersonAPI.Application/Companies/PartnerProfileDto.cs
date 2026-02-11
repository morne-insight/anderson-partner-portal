using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerProfileDto
    {
        public PartnerProfileDto()
        {
            Name = null!;
            FullDescription = null!;
            WebsiteUrl = null!;
            Capabilities = null!;
            Contacts = null!;
            ServiceTypeName = null!;
            ServiceSubTypes = null!;
            Industries = null!;
            Locations = null!;
            Opportunities = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public List<PartnerCapabilityDto> Capabilities { get; set; }
        public List<PartnerContactDto> Contacts { get; set; }
        public List<PartnerIndustryDto> Industries { get; set; }
        public List<PartnerLocationDto> Locations { get; set; }
        public List<PartnerOpportunityDto> Opportunities { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public List<PartnerServiceSubTypeDto> ServiceSubTypes { get; set; }

        public static PartnerProfileDto Create(
            Guid id,
            string name,
            string fullDescription,
            string websiteUrl,
            List<PartnerCapabilityDto> capabilities,
            List<PartnerContactDto> contacts,
            Guid? serviceTypeId,
            string serviceTypeName,
            List<PartnerServiceSubTypeDto> serviceSubTypes,
            List<PartnerIndustryDto> industries,
            List<PartnerLocationDto> locations,
            List<PartnerOpportunityDto> opportunities)
        {
            return new PartnerProfileDto
            {
                Id = id,
                Name = name,
                FullDescription = fullDescription,
                WebsiteUrl = websiteUrl,
                Capabilities = capabilities,
                Contacts = contacts,
                ServiceTypeId = serviceTypeId,
                ServiceTypeName = serviceTypeName
,
                ServiceSubTypes = serviceSubTypes,
                Industries = industries,
                Locations = locations,
                Opportunities = opportunities
            };
        }
    }
}