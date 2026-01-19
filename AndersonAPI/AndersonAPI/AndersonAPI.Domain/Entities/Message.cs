using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Message : BaseEntity
    {
        public Message(Guid opportunityId,
    string content,
    DateTimeOffset createdDate,
    string createdByUser,
    string? createdByPartner)
        {
            OpportunityId = opportunityId;
            Content = content;
            CreatedDate = createdDate;
            CreatedByUser = createdByUser;
            CreatedByPartner = createdByPartner;
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

        public string CreatedByUser { get; private set; }

        public string? CreatedByPartner { get; private set; }

        public void UpdateContent(string content)
        {
            Content = content;
        }
    }
}