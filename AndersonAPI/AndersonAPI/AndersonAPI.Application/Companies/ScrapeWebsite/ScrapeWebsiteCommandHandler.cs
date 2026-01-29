using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Application.Companies.CreateCompany;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.ScrapeWebsite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class ScrapeWebsiteCommandHandler : IRequestHandler<ScrapeWebsiteCommand, Guid>
    {
        private readonly IAgentService _agentService;
        private readonly IWebsiteScrapingService _websiteScrapingService;
        private readonly IMediator _mediator;
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public ScrapeWebsiteCommandHandler(
            IAgentService agentService,
            IWebsiteScrapingService websiteScrapingService,
            IMediator mediator,
            ICompanyRepository companyRepository)
        {
            _agentService = agentService;
            _websiteScrapingService = websiteScrapingService;
            _mediator = mediator;
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<Guid> Handle(ScrapeWebsiteCommand request, CancellationToken cancellationToken)
        {
            Company? company = null;

            if (request.CompanyId.HasValue)
            {
                company = await _companyRepository.FindByIdAsync(request.CompanyId.Value, cancellationToken);
                if (company == null)
                {
                    throw new InvalidOperationException($"Company with ID {request.CompanyId} not found.");
                }
            }

            var websiteContent = await _websiteScrapingService.ScrapeWebsiteAsync(request.Url);
            var response = await _agentService.RunAsync("I have included a website's content, please give me a summary of who they are and what they do: " + websiteContent);

            var companyName = ExtractCompanyNameFromUrl(request.Url);

            if (request.CompanyId.HasValue && company != null)
            {
                company.SetFullDescription(response);
                await _companyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return company.Id;
            }
            if (!request.CompanyId.HasValue)
            {
                var command = new CreateCompanyCommand(
                    name: companyName,
                    shortDescription: "",
                    fullDescription: response,
                    websiteUrl: request.Url,
                    employeeCount: 0,
                    capabilities: new List<Guid>(),
                    industries: new List<Guid>(),
                    serviceTypeId: null
                );
                return await _mediator.Send(command, cancellationToken);
            }

            return Guid.Empty;
        }


        private static string ExtractCompanyNameFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var host = uri.Host;

                // Remove www. prefix if present
                if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                {
                    host = host.Substring(4);
                }

                // Split by dots and take the first part (domain name without TLD)
                var parts = host.Split('.');
                if (parts.Length > 0)
                {
                    var companyName = parts[0];
                    // Capitalize first letter
                    return char.ToUpper(companyName[0]) + companyName.Substring(1);
                }

                return host;
            }
            catch
            {
                return "Unknown Company";
            }
        }
    }
}