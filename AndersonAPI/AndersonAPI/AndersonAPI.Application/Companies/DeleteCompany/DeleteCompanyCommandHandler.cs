using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.DeleteCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id, cancellationToken);
            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }


            _companyRepository.Remove(company);
        }
    }
}