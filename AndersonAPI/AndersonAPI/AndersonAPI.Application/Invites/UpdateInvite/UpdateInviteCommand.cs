using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Invites.UpdateInvite
{
    public class UpdateInviteCommand : IRequest, ICommand
    {
        public UpdateInviteCommand(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}