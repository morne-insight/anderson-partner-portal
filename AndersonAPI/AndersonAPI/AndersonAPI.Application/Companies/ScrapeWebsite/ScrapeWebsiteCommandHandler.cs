using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.ScrapeWebsite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class ScrapeWebsiteCommandHandler : IRequestHandler<ScrapeWebsiteCommand>
    {
        [IntentManaged(Mode.Merge)]
        public ScrapeWebsiteCommandHandler()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(ScrapeWebsiteCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implement Handle (ScrapeWebsiteCommandHandler) functionality
            throw new NotImplementedException("Your implementation here...");
        }
    }
}