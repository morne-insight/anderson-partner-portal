using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.SetCapabilitiesOpportunity
{
    public class SetCapabilitiesOpportunityCommand : IRequest, ICommand
    {
        public SetCapabilitiesOpportunityCommand(Guid id, List<Guid> capabilityIds)
        {
            Id = id;
            CapabilityIds = capabilityIds;
        }

        public Guid Id { get; set; }
        public List<Guid> CapabilityIds { get; set; }
    }
}