using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.AddMessageOppertunity
{
    public class AddMessageOppertunityCommand : IRequest, ICommand
    {
        public AddMessageOppertunityCommand(Guid oppertunityId,
            string content,
            DateTimeOffset createdDate,
            string createdByUser,
            string? createdByPartner)
        {
            OppertunityId = oppertunityId;
            Content = content;
            CreatedDate = createdDate;
            CreatedByUser = createdByUser;
            CreatedByPartner = createdByPartner;
        }

        public Guid OppertunityId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string? CreatedByPartner { get; set; }
    }
}