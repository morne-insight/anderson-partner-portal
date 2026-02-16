using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityViewDto
    {
        public OpportunityViewDto()
        {
            CompanyName = null!;
            Title = null!;
            FullDescription = null!;
            Country = null!;
            OpportunityType = null!;
            ServiceTypes = null!;
            InterestedPartners = null!;
            Capabilities = null!;
            Industries = null!;
        }

        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string FullDescription { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public string Country { get; set; }
        public string OpportunityType { get; set; }
        public List<OpportunityCapabilityDto> Capabilities { get; set; }
        public List<OpportunityIndustryDto> Industries { get; set; }
        public List<OpportunityServiceTypeDto> ServiceTypes { get; set; }
        public OpportunityStatus Status { get; set; }
        public List<OpportunityViewPartnerDto> InterestedPartners { get; set; }
        public Guid CompanyId { get; set; }

        public static OpportunityViewDto Create(
            Guid id,
            Guid companyId,
            string companyName,
            string title,
            string fullDescription,
            DateTimeOffset createdDate,
            DateOnly? deadline,
            string country,
            string opportunityType,
            List<OpportunityServiceTypeDto> serviceTypes,
            List<OpportunityViewPartnerDto> interestedPartners,
            List<OpportunityCapabilityDto> capabilities,
            List<OpportunityIndustryDto> industries,
            OpportunityStatus status)
        {
            return new OpportunityViewDto
            {
                Id = id,
                CompanyId = companyId
,
                CompanyName = companyName,
                Title = title,
                FullDescription = fullDescription,
                CreatedDate = createdDate,
                Deadline = deadline,
                Country = country,
                OpportunityType = opportunityType,
                ServiceTypes = serviceTypes,
                InterestedPartners = interestedPartners
,
                Capabilities = capabilities,
                Industries = industries,
                Status = status
            };
        }
    }
}