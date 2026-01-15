using AndersonAPI.Domain;
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
            ShortDescription = null!;
            FullDescription = null!;
        }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateOnly? Deadline { get; set; }
        public OppertunityStatus Status { get; set; }
        public Guid OppertunityTypeId { get; set; }
        public Guid CountryId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }

        public static OppertunityDto Create(
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            OppertunityStatus status,
            Guid oppertunityTypeId,
            Guid countryId,
            Guid companyId,
            Guid id,
            int order)
        {
            return new OppertunityDto
            {
                Title = title,
                ShortDescription = shortDescription,
                FullDescription = fullDescription,
                Deadline = deadline,
                Status = status,
                OppertunityTypeId = oppertunityTypeId,
                CountryId = countryId,
                CompanyId = companyId,
                Id = id,
                Order = order
            };
        }
    }
}