using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Message : BaseEntity
    {
        public Message(Guid opportunityId,
    string content,
    DateTimeOffset createdDate,
    Guid createdByUserId,
    string createdByUser,
    string? createdByPartner,
    bool isOwnMessage)
        {
            OpportunityId = opportunityId;
            Content = content;
            CreatedDate = createdDate;
            CreatedByUserId = createdByUserId;
            CreatedByUser = createdByUser;
            CreatedByPartner = createdByPartner;
            IsOwnMessage = isOwnMessage;
        }

        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Message()
        {
            Content = null!;
            CreatedByUser = null!;
        }

        public Guid OpportunityId { get; private set; }

        public string Content { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Guid CreatedByUserId { get; private set; }

        public string CreatedByUser { get; private set; }

        public string? CreatedByPartner { get; private set; }

        public bool IsOwnMessage { get; private set; }

        public void UpdateContent(string content)
        {
            Content = content;
        }
    }
}