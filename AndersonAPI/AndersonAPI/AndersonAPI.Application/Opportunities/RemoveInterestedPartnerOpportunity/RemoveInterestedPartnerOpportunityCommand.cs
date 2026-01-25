using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.RemoveInterestedPartnerOpportunity
{
    public class RemoveInterestedPartnerOpportunityCommand : IRequest, ICommand
    {
        public RemoveInterestedPartnerOpportunityCommand(Guid id, Guid partnerId)
        {
            Id = id;
            PartnerId = partnerId;
        }

        public Guid Id { get; set; }
        public Guid PartnerId { get; set; }
    }
}