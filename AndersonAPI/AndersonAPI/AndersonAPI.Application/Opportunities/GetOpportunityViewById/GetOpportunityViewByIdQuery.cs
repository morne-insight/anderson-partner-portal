using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.GetOpportunityViewById
{
    public class GetOpportunityViewByIdQuery : IRequest<OpportunityViewDto>, IQuery
    {
        public GetOpportunityViewByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}