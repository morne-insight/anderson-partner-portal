using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerContactDto
    {
        public PartnerContactDto()
        {
            FirstName = null!;
            LastName = null!;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? CompanyPosition { get; set; }

        public static PartnerContactDto Create(
            Guid id,
            string firstName,
            string lastName,
            string? emailAddress,
            string? companyPosition)
        {
            return new PartnerContactDto
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                CompanyPosition = companyPosition
            };
        }
    }
}