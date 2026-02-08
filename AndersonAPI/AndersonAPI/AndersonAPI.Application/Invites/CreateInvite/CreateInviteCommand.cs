using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Invites.CreateInvite
{
    public class CreateInviteCommand : IRequest<Guid>, ICommand
    {
        public CreateInviteCommand(string name, string email, Guid companyId)
        {
            Name = name;
            Email = email;
            CompanyId = companyId;
        }

        public string Name { get; set; }

        public string Email { get; set; }
        public Guid CompanyId { get; set; }
    }
}