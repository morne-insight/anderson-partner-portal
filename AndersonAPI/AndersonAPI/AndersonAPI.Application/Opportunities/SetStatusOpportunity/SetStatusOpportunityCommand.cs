using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.SetStatusOpportunity
{
    public class SetStatusOpportunityCommand : IRequest, ICommand
    {
        public SetStatusOpportunityCommand(Guid id, OpportunityStatus statusOpportunityStatus)
        {
            Id = id;
            StatusOpportunityStatus = statusOpportunityStatus;
        }

        public Guid Id { get; set; }
        public OpportunityStatus StatusOpportunityStatus { get; set; }
    }
}