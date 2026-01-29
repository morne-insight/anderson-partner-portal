using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetHealth
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetHealthQueryHandler : IRequestHandler<GetHealthQuery, string>
    {
        [IntentManaged(Mode.Merge)]
        public GetHealthQueryHandler()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<string> Handle(GetHealthQuery request, CancellationToken cancellationToken)
        {
            // TODO: Implement Handle (GetHealthQueryHandler) functionality// TODO: Implement Handle (GetHealthHandler) functionality
            throw new NotImplementedException("Your implementation here...");
        }
    }
}