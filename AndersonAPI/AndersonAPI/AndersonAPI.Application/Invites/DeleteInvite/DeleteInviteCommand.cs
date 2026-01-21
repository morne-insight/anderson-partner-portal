using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Invites.DeleteInvite
{
    public class DeleteInviteCommand : IRequest, ICommand
    {
        public DeleteInviteCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}