using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.GetOpportunityMessagesById
{
    public class GetOpportunityMessagesByIdQuery : IRequest<List<OpportunityMessageDto>>, IQuery
    {
        public GetOpportunityMessagesByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}