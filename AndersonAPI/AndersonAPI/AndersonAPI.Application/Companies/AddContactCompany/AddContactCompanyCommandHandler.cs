using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.AddContactCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AddContactCompanyCommandHandler : IRequestHandler<AddContactCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public AddContactCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(AddContactCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id, cancellationToken);
            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }

            company.AddContact(
                request.FirstName,
                request.LastName,
                request.EmailAddress,
                request.CompanyPosition);
        }
    }
}