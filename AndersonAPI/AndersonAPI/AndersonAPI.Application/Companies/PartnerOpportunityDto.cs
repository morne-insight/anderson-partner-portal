using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerOpportunityDto
    {
        public PartnerOpportunityDto()
        {
            Title = null!;
            ShortDescription = null!;
            ServiceTypes = null!;
            Country = null!;
            OpportunityType = null!;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public List<PartnerServiceTypeDto> ServiceTypes { get; set; }
        public string Country { get; set; }
        public DateOnly? Deadline { get; set; }
        public OpportunityStatus Status { get; set; }
        public Guid CompanyId { get; set; }
        public string OpportunityType { get; set; }

        public static PartnerOpportunityDto Create(
            Guid id,
            string title,
            string shortDescription,
            List<PartnerServiceTypeDto> serviceTypes,
            string country,
            DateOnly? deadline,
            OpportunityStatus status,
            Guid companyId,
            string opportunityType)
        {
            return new PartnerOpportunityDto
            {
                Id = id,
                Title = title,
                ShortDescription = shortDescription,
                ServiceTypes = serviceTypes,
                Country = country,
                Deadline = deadline,
                Status = status,
                CompanyId = companyId,
                OpportunityType = opportunityType
            };
        }
    }
}