using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.RemoveContactCompany
{
    public class RemoveContactCompanyCommand : IRequest, ICommand
    {
        public RemoveContactCompanyCommand(Guid id, Guid contactId)
        {
            Id = id;
            ContactId = contactId;
        }

        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
    }
}