using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Application.Companies;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.User.GetContactsByUserId
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetContactsByUserIdHandler : IRequestHandler<GetContactsByUserId, List<UserContact>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public GetContactsByUserIdHandler(
            ICurrentUserService currentUserService,
            ICompanyRepository companyRepository)
        {
            _currentUserService = currentUserService;
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<List<UserContact>> Handle(GetContactsByUserId request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetAsync();
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("Current user is not authenticated.");
            }

            var companies = await _companyRepository.FindAllAsync(
                c => c.ApplicationIdentityUsers.Any(u => u.Id == currentUser.Id.ToString()),
                queryOptions => queryOptions,
                cancellationToken);

            var contacts = companies
                .SelectMany(company => company.Contacts.Select(contact => UserContact.Create(
                    contactId: contact.Id,
                    firstName: contact.FirstName,
                    lastName: contact.LastName,
                    emailAddress: contact.EmailAddress,
                    companyId: contact.CompanyId,
                    companyPosition: contact.CompanyPosition,
                    name: company.Name,
                    websiteUrl: company.WebsiteUrl)))
                .ToList();

            return contacts;

        }
    }
}