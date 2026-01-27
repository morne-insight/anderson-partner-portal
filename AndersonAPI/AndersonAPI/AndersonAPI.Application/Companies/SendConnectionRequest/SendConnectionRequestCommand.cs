using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.SendConnectionRequest
{
    public class SendConnectionRequestCommand : IRequest, ICommand
    {
        public SendConnectionRequestCommand(Guid contactId, Guid companyId, Guid partnerId, string message)
        {
            ContactId = contactId;
            CompanyId = companyId;
            PartnerId = partnerId;
            Message = message;
        }

        public Guid ContactId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid PartnerId { get; set; }
        public string Message { get; set; }
    }
}