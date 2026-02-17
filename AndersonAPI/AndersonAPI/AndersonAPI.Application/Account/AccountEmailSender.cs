using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using Microsoft.Extensions.Configuration;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.AspNetCore.Identity.AccountController.AccountEmailSender", Version = "1.0")]

namespace AndersonAPI.Application.Account;

[IntentManaged(Mode.Fully, Body = Mode.Merge)]
public class AccountEmailSender : IAccountEmailSender
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AccountEmailSender(IEmailService emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
    }

    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public async Task SendEmailConfirmationRequest(string email, string userId, string code)
    {
        var baseUrl = _configuration["AppSettings:ClientUrl"] ?? "https://andersen.partners";
        var verificationUrl = $"{baseUrl}/confirm-email/{userId}/{code}";

        var htmlBody = await LoadEmailConfirmationTemplateAsync(
            userName: email,
            verificationUrl: verificationUrl);

        await _emailService.SendEmailAsync(
            to: email,
            subject: "Confirm your email address",
            body: htmlBody,
            isHtml: true);
    }

    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public async Task SendPasswordResetCode(string email, string userId, string resetCode)
    {
        var baseUrl = _configuration["AppSettings:ClientUrl"] ?? "http://localhost:3000";
        var resetUrl = $"{baseUrl}/reset-password/{email}/{resetCode}";

        var htmlBody = await LoadPasswordResetTemplateAsync(
            userName: email,
            verificationUrl: resetUrl);

        await _emailService.SendEmailAsync(
            to: email,
            subject: "Reset your password",
            body: htmlBody,
            isHtml: true);
    }

    private async Task<string> LoadEmailConfirmationTemplateAsync(
        string userName,
        string verificationUrl)
    {
        var templatePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Common",
            "Templates",
            "AccountConfirmation.html");

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Email template not found at: {templatePath}");
        }

        var template = await File.ReadAllTextAsync(templatePath);

        var replacements = new Dictionary<string, string>
        {
            { "{{product_or_company_name}}", "Anderson Consulting Group" },
            { "{{user_name}}", userName },
            { "{{verification_url}}", verificationUrl },
            { "{{link_expiry_human}}", "24 hours" },
            { "{{year}}", DateTime.UtcNow.Year.ToString() }
        };

        foreach (var replacement in replacements)
        {
            template = template.Replace(replacement.Key, replacement.Value);
        }

        return template;
    }

    private async Task<string> LoadPasswordResetTemplateAsync(
        string userName,
        string verificationUrl)
    {
        var templatePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Common",
            "Templates",
            "ResetPassword.html");

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Email template not found at: {templatePath}");
        }

        var template = await File.ReadAllTextAsync(templatePath);

        var replacements = new Dictionary<string, string>
        {
            { "{{product_or_company_name}}", "Anderson Consulting Group" },
            { "{{user_name}}", userName },
            { "{{verification_url}}", verificationUrl },
            { "{{link_expiry_human}}", "1 hour" },
            { "{{year}}", DateTime.UtcNow.Year.ToString() }
        };

        foreach (var replacement in replacements)
        {
            template = template.Replace(replacement.Key, replacement.Value);
        }

        return template;
    }
}