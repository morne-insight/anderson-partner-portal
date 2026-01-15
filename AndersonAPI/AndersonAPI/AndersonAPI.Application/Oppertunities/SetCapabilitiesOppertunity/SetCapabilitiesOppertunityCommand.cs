using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.SetCapabilitiesOppertunity
{
    public class SetCapabilitiesOppertunityCommand : IRequest, ICommand
    {
        public SetCapabilitiesOppertunityCommand(Guid id, List<Guid> capabilityIds)
        {
            Id = id;
            CapabilityIds = capabilityIds;
        }

        public Guid Id { get; set; }
        public List<Guid> CapabilityIds { get; set; }
    }
}