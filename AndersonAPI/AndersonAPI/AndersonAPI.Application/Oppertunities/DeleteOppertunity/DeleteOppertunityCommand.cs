using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.DeleteOppertunity
{
    public class DeleteOppertunityCommand : IRequest, ICommand
    {
        public DeleteOppertunityCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}