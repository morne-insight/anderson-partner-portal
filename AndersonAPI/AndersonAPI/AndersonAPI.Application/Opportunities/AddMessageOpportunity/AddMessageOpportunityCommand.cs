using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.AddMessageOpportunity
{
    public class AddMessageOpportunityCommand : IRequest, ICommand
    {
        public AddMessageOpportunityCommand(Guid opportunityId,
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

        public Guid OpportunityId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string? CreatedByPartner { get; set; }
        public Guid CreatedByUserId { get; set; }
        public bool IsOwnMessage { get; set; }
    }
}