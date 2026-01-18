using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.RemoveMessageOpportunity
{
    public class RemoveMessageOpportunityCommand : IRequest, ICommand
    {
        public RemoveMessageOpportunityCommand(Guid id, Guid messageId)
        {
            Id = id;
            MessageId = messageId;
        }

        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
    }
}