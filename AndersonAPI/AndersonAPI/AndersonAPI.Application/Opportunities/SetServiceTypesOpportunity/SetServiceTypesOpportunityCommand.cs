using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.SetServiceTypesOpportunity
{
    public class SetServiceTypesOpportunityCommand : IRequest, ICommand
    {
        public SetServiceTypesOpportunityCommand(Guid id, List<Guid> serviceTypeIds)
        {
            Id = id;
            ServiceTypeIds = serviceTypeIds;
        }

        public Guid Id { get; set; }
        public List<Guid> ServiceTypeIds { get; set; }
    }
}