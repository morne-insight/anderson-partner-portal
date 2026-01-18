using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.SetIndustriesCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetIndustriesCompanyCommandHandler : IRequestHandler<SetIndustriesCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public SetIndustriesCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetIndustriesCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(
                request.Id,
                queryOptions => queryOptions.Include(c => c.Industries),
                cancellationToken);

            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }

            var industries = await _companyRepository.GetIndustriesByIdsAsync(request.IndustryIds, cancellationToken);
            company.SetIndustries(industries);
        }
    }
}