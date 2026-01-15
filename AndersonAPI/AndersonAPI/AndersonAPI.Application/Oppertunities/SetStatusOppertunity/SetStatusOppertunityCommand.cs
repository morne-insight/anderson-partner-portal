using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.SetStatusOppertunity
{
    public class SetStatusOppertunityCommand : IRequest, ICommand
    {
        public SetStatusOppertunityCommand(Guid id, OppertunityStatus status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; set; }
        public OppertunityStatus Status { get; set; }
    }
}