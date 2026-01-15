using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetInterestedPartnersOppertunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetInterestedPartnersOppertunityCommandHandler : IRequestHandler<SetInterestedPartnersOppertunityCommand>
    {
        private readonly IOppertunityRepository _oppertunityRepository;
        private readonly ICompanyRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public SetInterestedPartnersOppertunityCommandHandler(
            IOppertunityRepository oppertunityRepository,
            ICompanyRepository companyProfileRepository)
        {
            _oppertunityRepository = oppertunityRepository;
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetInterestedPartnersOppertunityCommand request, CancellationToken cancellationToken)
        {
            var oppertunity = await _oppertunityRepository
                .FindByIdAsync(
                    request.Id,
                    queryOptions => queryOptions.Include(o => o.InterestedPartners),
                    cancellationToken);

            if (oppertunity is null)
            {
                throw new NotFoundException($"Could not find Oppertunity '{request.Id}'");
            }

            var distinctIds = request.CompanyIds.Distinct().ToList();

            // 2. Load the company profiles we want to associate
            var companies = await _companyProfileRepository.FindAllAsync(
                x => distinctIds.Contains(x.Id),
                cancellationToken);

            oppertunity.SetInterestedPartners(companies);
        }
    }
}