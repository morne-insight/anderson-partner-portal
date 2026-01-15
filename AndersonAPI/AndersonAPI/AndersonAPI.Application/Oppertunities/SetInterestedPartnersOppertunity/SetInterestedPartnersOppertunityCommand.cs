using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.SetInterestedPartnersOppertunity
{
    public class SetInterestedPartnersOppertunityCommand : IRequest, ICommand
    {
        public SetInterestedPartnersOppertunityCommand(Guid id, List<Guid> companyIds)
        {
            Id = id;
            CompanyIds = companyIds;
        }

        public Guid Id { get; set; }
        public List<Guid> CompanyIds { get; set; }
    }
}