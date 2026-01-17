using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record PartnerReviewDto
    {
        public PartnerReviewDto()
        {
            Comment = null!;
            ApplicationIdentityUserId = null!;
        }

        public Guid Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string ApplicationIdentityUserId { get; set; }
        public Guid ReviewerCompanyId { get; set; }
        public int Order { get; set; }
        public EntityState State { get; set; }

        public static PartnerReviewDto Create(
            Guid id,
            string comment,
            int rating,
            string applicationIdentityUserId,
            Guid reviewerCompanyId,
            int order,
            EntityState state)
        {
            return new PartnerReviewDto
            {
                Id = id,
                Comment = comment,
                Rating = rating,
                ApplicationIdentityUserId = applicationIdentityUserId,
                ReviewerCompanyId = reviewerCompanyId,
                Order = order,
                State = state
            };
        }
    }
}