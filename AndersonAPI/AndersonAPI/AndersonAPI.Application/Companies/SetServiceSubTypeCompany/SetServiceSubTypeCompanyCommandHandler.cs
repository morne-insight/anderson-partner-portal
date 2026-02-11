using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetServiceSubTypeCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetServiceSubTypeCompanyCommandHandler : IRequestHandler<SetServiceSubTypeCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public SetServiceSubTypeCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetServiceSubTypeCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository
                .FindByIdAsync(
                    request.Id, 
                    queryOptions => queryOptions.Include(c => c.ServiceSubTypes), 
                    cancellationToken);

            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }

            var serviceSubTypes = await _companyRepository.GetServiceSubTypesByIdsAsync(request.ServiceSubTypeIds, cancellationToken);
            company.SetServiceSubTypes(serviceSubTypes);
        }
    }
}