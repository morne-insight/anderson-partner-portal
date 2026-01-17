using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.ScrapeWebsite
{
    public class ScrapeWebsiteCommand : IRequest, ICommand
    {
        public ScrapeWebsiteCommand(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}