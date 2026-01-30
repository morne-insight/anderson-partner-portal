using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetApiHealth
{
    /// <summary>
    /// Hits the database via CQRS pipeline without authentication
    /// </summary>
    public class GetApiHealthQuery : IRequest<string>, IQuery
    {
        public GetApiHealthQuery()
        {
        }
    }
}