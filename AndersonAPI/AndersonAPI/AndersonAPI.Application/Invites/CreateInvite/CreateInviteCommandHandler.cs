using System.Text;
using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.Extensions.Configuration;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Invites.CreateInvite
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateInviteCommandHandler : IRequestHandler<CreateInviteCommand, Guid>
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationIdentityUserRepository _applicationIdentityUserRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        [IntentManaged(Mode.Merge)]
        public CreateInviteCommandHandler(
            IInviteRepository inviteRepository,
            ICompanyRepository companyRepository,
            ICurrentUserService currentUserService,
            IApplicationIdentityUserRepository applicationIdentityUserRepository,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _inviteRepository = inviteRepository;
            _companyRepository = companyRepository;
            _currentUserService = currentUserService;
            _applicationIdentityUserRepository = applicationIdentityUserRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<Guid> Handle(CreateInviteCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetAsync();
            if (currentUser == null || !currentUser.Id.HasValue)
            {
                throw new UnauthorizedAccessException("Current user is not authenticated.");
            }

            var invitingUser = await _applicationIdentityUserRepository.FindByIdAsync(currentUser.Id.Value.ToString(), cancellationToken);
            if (invitingUser == null)
            {
                throw new NotFoundException($"Inviting user with ID {currentUser.Id} not found.");
            }

            var invitedUser = await _applicationIdentityUserRepository.FindAsync(x => x.Email == request.Email, cancellationToken);

            var company = await _companyRepository.FindByIdAsync(request.CompanyId, cancellationToken);
            if (company == null)
            {
                throw new NotFoundException($"Company with ID {request.CompanyId} not found.");
            }

            var invite = new Invite(
                name: request.Name,
                email: request.Email,
                companyId: request.CompanyId);

            _inviteRepository.Add(invite);
            await _inviteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var baseUrl = _configuration["AppSettings:ClientUrl"] ?? "http://localhost:3000";

            string invitationUrl;
            if (invitedUser == null)
            {
                invitationUrl = $"{baseUrl}/register"; 
            }
            else 
            { 
                invitationUrl = $"{baseUrl}/accept-invite/{invite.Id}/{invitedUser.Id}";
            }

            var htmlBody = await LoadInviteTemplateAsync(
                    invitingName: invitingUser.Name ?? "the site manager",
                    invitedName: request.Name,
                    inviteUrl: invitationUrl,
                    companyName: company.Name,
                    requiresRegistration: invitedUser == null);

            await _emailService.SendEmailAsync(
                to: request.Email,
                subject: $"You're invited to join {company.Name} on the Anderson partner portal",
                body: htmlBody,
                isHtml: true);

            return invite.Id;
        }

        [IntentManaged(Mode.Ignore)]
        private async Task<string> LoadInviteTemplateAsync(
            string invitingName,
            string invitedName,
            string inviteUrl,
            string companyName,
            bool requiresRegistration)
        {
            var templatePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Common",
                "Templates",
                "CompanyInvite.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException("Email template not found.", templatePath);
            }

            var template = await File.ReadAllTextAsync(templatePath);
            
            string registrationMessage = requiresRegistration
                ? $"<p>You will be required to register to join {companyName}.</p>"
                : "";

            var replacements = new Dictionary<string, string>
            {
                { "{{product_or_company_name}}", "Anderson Consulting Group" },
                { "{{inviting_user_name}}", invitingName },
                { "{{invited_user_name}}", invitedName },
                { "{{company_name}}", companyName },
                { "{{registration_message}}", registrationMessage },
                { "{{invitation_url}}", inviteUrl },
                { "{{link_expiry_human}}", "24 hours" },
                { "{{year}}", DateTime.UtcNow.Year.ToString() }
            };

            foreach (var placeholder in replacements.Keys)
            {
                template = template.Replace(placeholder, replacements[placeholder]);
            }

            return template;
        }
    }
}