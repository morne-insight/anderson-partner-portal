using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SendConnectionRequest
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SendConnectionRequestCommandHandler : IRequestHandler<SendConnectionRequestCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailService _emailService;

        [IntentManaged(Mode.Merge)]
        public SendConnectionRequestCommandHandler(
            ICompanyRepository companyRepository,
            IEmailService emailService)
        {
            _companyRepository = companyRepository;
            _emailService = emailService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SendConnectionRequestCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository
                .FindByIdAsync(
                    request.CompanyId,
                    queryOptions => queryOptions.Include(p => p.Contacts),
                    cancellationToken);

            var partner = await _companyRepository
                .FindByIdAsync(
                    request.PartnerId,
                    queryOptions => queryOptions.Include(p => p.Contacts),
                    cancellationToken);

            var contact = company?.Contacts.FirstOrDefault(c => c.Id == request.ContactId);

            if (company == null || partner == null || contact == null)
            {
                throw new InvalidOperationException("Company, partner, or contact not found.");
            }

            //var partnerContact = partner.Contacts.FirstOrDefault();
            //if (partnerContact == null || string.IsNullOrEmpty(partnerContact.EmailAddress))
            //{
            //    throw new InvalidOperationException("Partner contact email not found.");
            //}

            foreach (var partnerContact in partner.Contacts)
            {
                if (string.IsNullOrEmpty(partnerContact.EmailAddress))
                {
                    continue; // Skip contacts without email addresses
                }
                var htmlBody = await LoadTemplateAndReplaceTagsAsync(
                    partnerContact.FirstName != null || partnerContact.LastName != null ? $"{partnerContact.FirstName} {partnerContact.LastName}" : "Name not available",
                    company.Name,
                    contact.FirstName != null || contact.LastName != null ? $"{contact.FirstName} {contact.LastName}" : "Partner",
                    contact.EmailAddress ?? "",
                    request.Message);

                await _emailService.SendEmailAsync(
                    partnerContact.EmailAddress,
                    $"Partner Connect Request from {company.Name}",
                    htmlBody,
                    isHtml: true,
                    cancellationToken);
            }
        }

        private async Task<string> LoadTemplateAndReplaceTagsAsync(
            string userName,
            string partnerCompanyName,
            string contactName,
            string contactEmail,
            string connectionMessage)
        {
            var templatePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Common",
                "Templates",
                "ConnectWithPartner.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Email template not found at: {templatePath}");
            }

            var template = await File.ReadAllTextAsync(templatePath);

            var replacements = new Dictionary<string, string>
            {
                { "{{product_or_company_name}}", "Anderson Consulting Group" },
                { "{{user_name}}", userName },
                { "{{partner_company_name}}", partnerCompanyName },
                { "{{connection_message}}", FormatMessageAsHtml(connectionMessage) },
                { "{{partner_name}}", contactName },
                { "{{partner_email}}", contactEmail },
                { "{{year}}", DateTime.UtcNow.Year.ToString() }
            };

            foreach (var replacement in replacements)
            {
                template = template.Replace(replacement.Key, replacement.Value);
            }

            return template;
        }

        private string FormatMessageAsHtml(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return "<p style=\"margin:0 0 14px 0; font-family: Inter, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif; font-size:16px; line-height:24px; color:#111111;\">No additional message provided.</p>";
            }

            var paragraphs = message.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(p => $"<p style=\"margin:0 0 14px 0; font-family: Inter, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif; font-size:16px; line-height:24px; color:#111111;\">{p}</p>");

            return string.Join("\n", paragraphs);
        }
    }
}