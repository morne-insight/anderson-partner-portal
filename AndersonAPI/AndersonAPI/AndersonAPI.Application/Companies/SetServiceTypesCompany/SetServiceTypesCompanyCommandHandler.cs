using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetServiceTypesCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetServiceTypesCompanyCommandHandler : IRequestHandler<SetServiceTypesCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public SetServiceTypesCompanyCommandHandler(ICompanyRepository companyRepository, IServiceTypeRepository serviceTypeRepository)
        {
            _companyRepository = companyRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetServiceTypesCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id, cancellationToken);
            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }

            var serviceTypes = await _companyRepository.GetServiceTypesByIdsAsync(request.ServiceTypeIds, cancellationToken);
            company.SetServiceTypes(serviceTypes);
        }
    }
}