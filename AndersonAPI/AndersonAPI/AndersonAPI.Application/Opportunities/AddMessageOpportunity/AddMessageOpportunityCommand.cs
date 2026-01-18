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
            string createdByUser,
            string? createdByPartner)
        {
            OpportunityId = opportunityId;
            Content = content;
            CreatedDate = createdDate;
            CreatedByUser = createdByUser;
            CreatedByPartner = createdByPartner;
        }

        public Guid OpportunityId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string? CreatedByPartner { get; set; }
    }
}