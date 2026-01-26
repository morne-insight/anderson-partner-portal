namespace AndersonAPI.Application.Common.Interfaces
{
    public interface IWebsiteScrapingService
    {
        Task<string> ScrapeWebsiteAsync(string url, CancellationToken cancellationToken = default);
    }
}
