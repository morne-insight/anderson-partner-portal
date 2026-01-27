namespace AndersonAPI.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default);

        Task SendEmailsAsync(IEnumerable<string> to, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default);
    }
}
