using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.CreateQuarterly
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateQuarterlyCommandHandler : IRequestHandler<CreateQuarterlyCommand, Guid>
    {
        [IntentManaged(Mode.Merge)]
        public CreateQuarterlyCommandHandler()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<Guid> Handle(CreateQuarterlyCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implement Handle (CreateQuarterlyCommandHandler) functionality
            throw new NotImplementedException("Your implementation here...");
        }
    }
}