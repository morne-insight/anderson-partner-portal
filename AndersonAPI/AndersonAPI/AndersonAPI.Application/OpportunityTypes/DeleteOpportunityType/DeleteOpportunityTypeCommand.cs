using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.OpportunityTypes.DeleteOpportunityType
{
    public class DeleteOpportunityTypeCommand : IRequest, ICommand
    {
        public DeleteOpportunityTypeCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}