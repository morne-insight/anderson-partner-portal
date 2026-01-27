using AndersonAPI.Application.Common.ConfigOptions;
using AndersonAPI.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace AndersonAPI.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtpOptions;
        public EmailService(IOptions<SmtpOptions> options)
        {
            _smtpOptions = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default)
        {
            using var smtpClient = new System.Net.Mail.SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
            {
                Credentials = new System.Net.NetworkCredential(_smtpOptions.Username, _smtpOptions.Password),
                EnableSsl = _smtpOptions.EnableSsl,
                UseDefaultCredentials = false
            };

            var mailMessage = new System.Net.Mail.MailMessage
            {
                //From = new System.Net.Mail.MailAddress(_smtpOptions.FromAddress, _smtpOptions.FromName),
                From = new System.Net.Mail.MailAddress(_smtpOptions.FromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }

        public async Task SendEmailsAsync(IEnumerable<string> to, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default)
        {
            using var smtpClient = new System.Net.Mail.SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
            {
                Credentials = new System.Net.NetworkCredential(_smtpOptions.Username, _smtpOptions.Password),
                EnableSsl = _smtpOptions.EnableSsl,
                UseDefaultCredentials = false
            };
            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_smtpOptions.FromAddress, _smtpOptions.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            foreach (var recipient in to)
            {
                mailMessage.To.Add(recipient);
            }
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
