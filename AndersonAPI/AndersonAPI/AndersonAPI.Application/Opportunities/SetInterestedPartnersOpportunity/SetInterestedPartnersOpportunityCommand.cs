using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.SetInterestedPartnersOpportunity
{
    public class SetInterestedPartnersOpportunityCommand : IRequest, ICommand
    {
        public SetInterestedPartnersOpportunityCommand(Guid id, List<Guid> companyIds)
        {
            Id = id;
            CompanyIds = companyIds;
        }

        public Guid Id { get; set; }
        public List<Guid> CompanyIds { get; set; }
    }
}