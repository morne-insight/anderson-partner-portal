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
            CompanyServiceType = null!;
            Title = null!;
            FullDescription = null!;
            Country = null!;
            OpportunityType = null!;
            Capabilities = null!;
            Industries = null!;
            ServiceTypes = null!;
            InterestedPartners = null!;
        }

        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyServiceType { get; set; }
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
            string companyServiceType,
            string title,
            string fullDescription,
            DateTimeOffset createdDate,
            DateOnly? deadline,
            string country,
            string opportunityType,
            List<OpportunityCapabilityDto> capabilities,
            List<OpportunityIndustryDto> industries,
            List<OpportunityServiceTypeDto> serviceTypes,
            OpportunityStatus status,
            List<OpportunityViewPartnerDto> interestedPartners)
        {
            return new OpportunityViewDto
            {
                Id = id,
                CompanyId = companyId
,
                CompanyName = companyName,
                CompanyServiceType = companyServiceType,
                Title = title,
                FullDescription = fullDescription,
                CreatedDate = createdDate,
                Deadline = deadline,
                Country = country,
                OpportunityType = opportunityType,
                Capabilities = capabilities,
                Industries = industries,
                ServiceTypes = serviceTypes,
                Status = status,
                InterestedPartners = interestedPartners
            };
        }
    }
}