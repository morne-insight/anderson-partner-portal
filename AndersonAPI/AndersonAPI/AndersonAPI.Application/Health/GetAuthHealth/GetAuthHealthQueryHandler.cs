using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetAuthHealth
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetAuthHealthQueryHandler : IRequestHandler<GetAuthHealthQuery, string>
    {
        [IntentManaged(Mode.Merge)]
        public GetAuthHealthQueryHandler()
        {
        }

        /// <summary>
        /// Hits the database via CQRS pipeline with authentication
        /// </summary>
        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<string> Handle(GetAuthHealthQuery request, CancellationToken cancellationToken)
        {
            // TODO: Implement Handle (GetAuthHealthQueryHandler) functionality
            throw new NotImplementedException("Your implementation here...");
        }
    }
}