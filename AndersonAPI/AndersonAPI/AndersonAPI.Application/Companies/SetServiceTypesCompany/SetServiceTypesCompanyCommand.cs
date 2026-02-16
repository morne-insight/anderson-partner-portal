using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.SetServiceTypesCompany
{
    public class SetServiceTypesCompanyCommand : IRequest, ICommand
    {
        public SetServiceTypesCompanyCommand(Guid id, List<Guid> serviceTypeIds)
        {
            Id = id;
            ServiceTypeIds = serviceTypeIds;
        }

        public Guid Id { get; set; }
        public List<Guid> ServiceTypeIds { get; set; }
    }
}