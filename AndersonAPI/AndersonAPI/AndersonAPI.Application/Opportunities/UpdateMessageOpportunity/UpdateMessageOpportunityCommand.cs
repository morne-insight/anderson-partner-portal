using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.UpdateMessageOpportunity
{
    public class UpdateMessageOpportunityCommand : IRequest, ICommand
    {
        public UpdateMessageOpportunityCommand(Guid id, Guid messageId, string content)
        {
            Id = id;
            MessageId = messageId;
            Content = content;
        }

        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public string Content { get; set; }
    }
}