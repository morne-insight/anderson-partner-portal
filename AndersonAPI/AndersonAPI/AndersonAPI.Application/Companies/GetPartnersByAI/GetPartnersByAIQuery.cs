using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetPartnersByAI
{
    public class GetPartnersByAIQuery : IRequest<List<PartnerProfileListItem>>, IQuery
    {
        public GetPartnersByAIQuery(string query)
        {
            Query = query;
        }

        public string Query { get; set; }
    }
}