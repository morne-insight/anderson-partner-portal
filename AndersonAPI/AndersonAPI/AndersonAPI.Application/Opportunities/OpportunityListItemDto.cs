using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityListItemDto
    {
        public OpportunityListItemDto()
        {
            Title = null!;
            ShortDescription = null!;
            CompanyName = null!;
            Country = null!;
            OpportunityTypeString = null!;
            InterestedPartners = null!;
            Capabilities = null!;
        }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string OpportunityTypeString { get; set; }
        public List<InterestedPartnerDto> InterestedPartners { get; set; }
        public List<OpportunityCapabilityDto> Capabilities { get; set; }

        public static OpportunityListItemDto Create(
            Guid id,
            string title,
            string shortDescription,
            DateTimeOffset createdDate,
            DateOnly? deadline,
            Guid companyId,
            string companyName,
            int order,
            string country,
            string opportunityTypeString,
            List<InterestedPartnerDto> interestedPartners,
            List<OpportunityCapabilityDto> capabilities)
        {
            return new OpportunityListItemDto
            {
                Id = id,
                Title = title,
                ShortDescription = shortDescription,
                CreatedDate = createdDate,
                Deadline = deadline,
                CompanyId = companyId,
                CompanyName = companyName,
                Order = order
,
                Country = country,
                OpportunityTypeString = opportunityTypeString,
                InterestedPartners = interestedPartners
,
                Capabilities = capabilities
            };
        }
    }
}