using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.DeleteOpportunity
{
    public class DeleteOpportunityCommand : IRequest, ICommand
    {
        public DeleteOpportunityCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}