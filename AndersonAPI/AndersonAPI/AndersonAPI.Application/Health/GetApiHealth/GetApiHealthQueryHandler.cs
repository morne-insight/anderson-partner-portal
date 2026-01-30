using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetApiHealth
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetApiHealthQueryHandler : IRequestHandler<GetApiHealthQuery, string>
    {
        [IntentManaged(Mode.Merge)]
        public GetApiHealthQueryHandler()
        {
        }

        /// <summary>
        /// Hits the database via CQRS pipeline without authentication
        /// </summary>
        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<string> Handle(GetApiHealthQuery request, CancellationToken cancellationToken)
        {
            // TODO: Implement Handle (GetApiHealthQueryHandler) functionality// TODO: Implement Handle (GetApiHealthQueryHandler) functionality// TODO: Implement Handle (GetHealthQueryHandler) functionality// TODO: Implement Handle (GetHealthHandler) functionality
            throw new NotImplementedException("Your implementation here...");
        }
    }
}