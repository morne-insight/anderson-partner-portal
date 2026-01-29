using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.ScrapeWebsite
{
    public class ScrapeWebsiteCommand : IRequest<Guid>, ICommand
    {
        public ScrapeWebsiteCommand(Guid? companyId, string url)
        {
            CompanyId = companyId;
            Url = url;
        }

        public Guid? CompanyId { get; set; }

        public string Url { get; set; }
    }
}