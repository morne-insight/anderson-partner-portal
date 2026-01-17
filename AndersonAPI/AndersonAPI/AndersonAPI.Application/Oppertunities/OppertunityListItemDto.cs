using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities
{
    public record OppertunityListItemDto
    {
        public OppertunityListItemDto()
        {
            Title = null!;
            ShortDescription = null!;
            Country = null!;
            OppertunityType = null!;
            InterestedPartners = null!;
        }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string OppertunityType { get; set; }
        public List<InterestedPartnerDto> InterestedPartners { get; set; }

        public static OppertunityListItemDto Create(
            string title,
            string shortDescription,
            DateTimeOffset createdDate,
            DateOnly? deadline,
            Guid companyId,
            Guid id,
            int order,
            string country,
            string oppertunityType,
            List<InterestedPartnerDto> interestedPartners)
        {
            return new OppertunityListItemDto
            {
                Title = title,
                ShortDescription = shortDescription,
                CreatedDate = createdDate,
                Deadline = deadline,
                CompanyId = companyId,
                Id = id,
                Order = order
,
                Country = country,
                OppertunityType = oppertunityType,
                InterestedPartners = interestedPartners
            };
        }
    }
}