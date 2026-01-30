using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetAuthHealth
{
    /// <summary>
    /// Hits the database via CQRS pipeline with authentication
    /// </summary>
    public class GetAuthHealthQuery : IRequest<string>, IQuery
    {
        public GetAuthHealthQuery()
        {
        }
    }
}