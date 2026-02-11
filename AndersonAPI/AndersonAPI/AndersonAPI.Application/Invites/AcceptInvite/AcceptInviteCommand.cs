using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Invites.AcceptInvite
{
    public class AcceptInviteCommand : IRequest, ICommand
    {
        public AcceptInviteCommand(Guid id, string userId)
        {
            Id = id;
            UserId = userId;
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }
    }
}