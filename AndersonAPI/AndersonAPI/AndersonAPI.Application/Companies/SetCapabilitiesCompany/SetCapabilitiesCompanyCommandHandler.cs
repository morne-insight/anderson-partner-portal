using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetCapabilitiesCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetCapabilitiesCompanyCommandHandler : IRequestHandler<SetCapabilitiesCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public SetCapabilitiesCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetCapabilitiesCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(
                request.Id, 
                queryOptions => queryOptions.Include(c => c.Capabilities),
                cancellationToken);

            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }

            var capabilities = await _companyRepository.GetCapabilitiesByIdsAsync(request.CapabilityIds, cancellationToken);
            company.SetCapabilities(capabilities);
        }
    }
}