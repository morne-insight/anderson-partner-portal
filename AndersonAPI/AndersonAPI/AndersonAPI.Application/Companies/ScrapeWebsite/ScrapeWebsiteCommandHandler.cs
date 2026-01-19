using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.ScrapeWebsite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class ScrapeWebsiteCommandHandler : IRequestHandler<ScrapeWebsiteCommand>
    {
        private readonly IAgentService _agentService;

        [IntentManaged(Mode.Merge)]
        public ScrapeWebsiteCommandHandler(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(ScrapeWebsiteCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implement Handle (ScrapeWebsiteCommandHandler) functionality
            var response = await _agentService.RunAsync("Can you please browse this website https://insightconsulting.co.za/ and give me a summary on what they do.");

        }
    }
}