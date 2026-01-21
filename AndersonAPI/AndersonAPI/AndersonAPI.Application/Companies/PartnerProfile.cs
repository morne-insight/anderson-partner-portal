using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerProfile
    {
        public PartnerProfile()
        {
            Name = null!;
            FullDescription = null!;
            WebsiteUrl = null!;
            Capabilities = null!;
            Contacts = null!;
            Industries = null!;
            Locations = null!;
            Opportunities = null!;
            Reviews = null!;
            ServiceTypeName = null!;
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
        public List<PartnerReviewDto> Reviews { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }

        public static PartnerProfile Create(
            Guid id,
            string name,
            string fullDescription,
            string websiteUrl,
            List<PartnerCapabilityDto> capabilities,
            List<PartnerContactDto> contacts,
            List<PartnerIndustryDto> industries,
            List<PartnerLocationDto> locations,
            List<PartnerOpportunityDto> opportunities,
            List<PartnerReviewDto> reviews,
            Guid? serviceTypeId,
            string serviceTypeName)
        {
            return new PartnerProfile
            {
                Id = id,
                Name = name,
                FullDescription = fullDescription,
                WebsiteUrl = websiteUrl,
                Capabilities = capabilities,
                Contacts = contacts,
                Industries = industries,
                Locations = locations,
                Opportunities = opportunities,
                Reviews = reviews
,
                ServiceTypeId = serviceTypeId,
                ServiceTypeName = serviceTypeName
            };
        }
    }
}