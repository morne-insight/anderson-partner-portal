using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities
{
    public record OpportunityMessageDto
    {
        public OpportunityMessageDto()
        {
            Content = null!;
            CreatedByUser = null!;
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string? CreatedByPartner { get; set; }
        public int Order { get; set; }
        public bool IsOwnMessage { get; set; }
        public Guid CreatedByUserId { get; set; }

        public static OpportunityMessageDto Create(
            Guid id,
            string content,
            DateTimeOffset createdDate,
            string createdByUser,
            string? createdByPartner,
            int order,
            bool isOwnMessage,
            Guid createdByUserId)
        {
            return new OpportunityMessageDto
            {
                Id = id,
                Content = content,
                CreatedDate = createdDate,
                CreatedByUser = createdByUser,
                CreatedByPartner = createdByPartner,
                Order = order,
                IsOwnMessage = isOwnMessage,
                CreatedByUserId = createdByUserId
            };
        }
    }
}