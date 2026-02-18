using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetServiceTypesCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetServiceTypesCompanyCommandHandler : IRequestHandler<SetServiceTypesCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public SetServiceTypesCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(SetServiceTypesCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository
                .FindByIdAsync(
                    request.Id, 
                    queryOptions => queryOptions.Include(x => x.ServiceTypes),
                    cancellationToken);

            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }

            var serviceTypes = await _companyRepository.GetServiceTypesByIdsAsync(request.ServiceTypeIds, cancellationToken);
            company.SetServiceTypes(serviceTypes);
        }
    }
}