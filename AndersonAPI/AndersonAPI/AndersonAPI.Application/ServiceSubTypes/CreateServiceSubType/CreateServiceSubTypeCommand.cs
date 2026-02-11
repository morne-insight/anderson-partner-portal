using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.ServiceSubTypes.CreateServiceSubType
{
    public class CreateServiceSubTypeCommand : IRequest<Guid>, ICommand
    {
        public CreateServiceSubTypeCommand(Guid serviceTypeId, string name, string description)
        {
            ServiceTypeId = serviceTypeId;
            Name = name;
            Description = description;
        }

        public Guid ServiceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}