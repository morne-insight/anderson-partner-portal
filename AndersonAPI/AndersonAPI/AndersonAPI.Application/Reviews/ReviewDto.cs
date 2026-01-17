using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Reviews
{
    public record ReviewDto
    {
        public ReviewDto()
        {
            Comment = null!;
            ApplicationIdentityUserId = null!;
        }

        public string Comment { get; set; }
        public int Rating { get; set; }
        public string ApplicationIdentityUserId { get; set; }
        public Guid ReviewerCompanyId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public EntityState State { get; set; }

        public static ReviewDto Create(
            string comment,
            int rating,
            string applicationIdentityUserId,
            Guid reviewerCompanyId,
            Guid id,
            int order,
            EntityState state)
        {
            return new ReviewDto
            {
                Comment = comment,
                Rating = rating,
                ApplicationIdentityUserId = applicationIdentityUserId,
                ReviewerCompanyId = reviewerCompanyId,
                Id = id,
                Order = order,
                State = state
            };
        }
    }
}