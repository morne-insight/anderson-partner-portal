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
            UserId = null!;
            ReviewerCompanyName = null!;
        }

        public string Comment { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
        public string? CreatedByUserName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid ReviewerCompanyId { get; set; }
        public string ReviewerCompanyName { get; set; }
        public Guid Id { get; set; }

        public static ReviewDto Create(
            Guid id,
            string comment,
            int rating,
            string userId,
            string? createdByUserName,
            DateTimeOffset createdDate,
            Guid reviewerCompanyId,
            string reviewerCompanyName)
        {
            return new ReviewDto
            {
                Id = id
,
                Comment = comment,
                Rating = rating,
                UserId = userId,
                CreatedByUserName = createdByUserName,
                CreatedDate = createdDate,
                ReviewerCompanyId = reviewerCompanyId,
                ReviewerCompanyName = reviewerCompanyName
            };
        }
    }
}