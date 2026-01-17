using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public record OppertunityDto
    {
        public OppertunityDto()
        {
            Title = null!;
            FullDescription = null!;
            Country = null!;
            OppertunityType = null!;
            InterestedPartners = null!;
            Capabilities = null!;
            Industries = null!;
            ServiceTypes = null!;
        }

        public string Title { get; set; }
        public string FullDescription { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string OppertunityType { get; set; }
        public List<InterestedPartnerDto> InterestedPartners { get; set; }
        public List<OppertunityCapabilityDto> Capabilities { get; set; }
        public List<OppertunityIndustryDto> Industries { get; set; }
        public List<OppertunityServiceTypeDto> ServiceTypes { get; set; }

        public static OppertunityDto Create(
            string title,
            string fullDescription,
            DateTimeOffset createdDate,
            DateOnly? deadline,
            Guid companyId,
            Guid id,
            int order,
            string country,
            string oppertunityType,
            List<InterestedPartnerDto> interestedPartners,
            List<OppertunityCapabilityDto> capabilities,
            List<OppertunityIndustryDto> industries,
            List<OppertunityServiceTypeDto> serviceTypes)
        {
            return new OppertunityDto
            {
                Title = title,
                FullDescription = fullDescription,
                CreatedDate = createdDate,
                Deadline = deadline,
                CompanyId = companyId,
                Id = id,
                Order = order,
                Country = country,
                OppertunityType = oppertunityType,
                InterestedPartners = interestedPartners,
                Capabilities = capabilities,
                Industries = industries,
                ServiceTypes = serviceTypes
            };
        }
    }
}