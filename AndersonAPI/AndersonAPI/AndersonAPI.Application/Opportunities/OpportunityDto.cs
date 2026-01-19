using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityDto
    {
        public OpportunityDto()
        {
            Title = null!;
            FullDescription = null!;
            Country = null!;
            OpportunityType = null!;
            Capabilities = null!;
            Industries = null!;
            ServiceTypes = null!;
            ShortDescription = null!;
        }

        public string Title { get; set; }
        public string FullDescription { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string OpportunityType { get; set; }
        public List<OpportunityCapabilityDto> Capabilities { get; set; }
        public List<OpportunityIndustryDto> Industries { get; set; }
        public List<OpportunityServiceTypeDto> ServiceTypes { get; set; }
        public OpportunityStatus Status { get; set; }
        public string ShortDescription { get; set; }
        public Guid OpportunityTypeId { get; set; }
        public Guid CountryId { get; set; }

        public static OpportunityDto Create(
            Guid id,
            string title,
            string fullDescription,
            DateTimeOffset createdDate,
            DateOnly? deadline,
            Guid companyId,
            string country,
            string opportunityType,
            List<OpportunityCapabilityDto> capabilities,
            List<OpportunityIndustryDto> industries,
            List<OpportunityServiceTypeDto> serviceTypes,
            OpportunityStatus status,
            string shortDescription,
            Guid opportunityTypeId,
            Guid countryId)
        {
            return new OpportunityDto
            {
                Id = id,
                Title = title,
                FullDescription = fullDescription,
                CreatedDate = createdDate,
                Deadline = deadline,
                CompanyId = companyId,
                Country = country,
                OpportunityType = opportunityType,
                Capabilities = capabilities,
                Industries = industries,
                ServiceTypes = serviceTypes,
                Status = status
,
                ShortDescription = shortDescription,
                OpportunityTypeId = opportunityTypeId,
                CountryId = countryId
            };
        }
    }
}