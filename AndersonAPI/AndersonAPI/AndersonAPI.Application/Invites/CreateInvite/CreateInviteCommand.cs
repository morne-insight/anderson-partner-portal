using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Invites.CreateInvite
{
    public class CreateInviteCommand : IRequest<Guid>, ICommand
    {
        public CreateInviteCommand(string email, Guid companyId)
        {
            Email = email;
            CompanyId = companyId;
        }

        public string Email { get; set; }
        public Guid CompanyId { get; set; }
    }
}