using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.SetServiceSubTypeCompany
{
    public class SetServiceSubTypeCompanyCommand : IRequest, ICommand
    {
        public SetServiceSubTypeCompanyCommand(Guid id, List<Guid> serviceSubTypeIds)
        {
            Id = id;
            ServiceSubTypeIds = serviceSubTypeIds;
        }

        public Guid Id { get; set; }
        public List<Guid> ServiceSubTypeIds { get; set; }
    }
}