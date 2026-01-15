using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.OppertunityTypes.DeleteOppertunityType
{
    public class DeleteOppertunityTypeCommand : IRequest, ICommand
    {
        public DeleteOppertunityTypeCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}