using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.GetOppertunities
{
    public class GetOppertunitiesQuery : IRequest<List<OppertunityListItemDto>>, IQuery
    {
        public GetOppertunitiesQuery()
        {
        }
    }
}