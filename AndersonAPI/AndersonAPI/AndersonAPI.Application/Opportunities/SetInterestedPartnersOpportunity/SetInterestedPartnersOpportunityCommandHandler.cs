using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetInterestedPartnersOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetInterestedPartnersOpportunityCommandHandler : IRequestHandler<SetInterestedPartnersOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ICompanyRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public SetInterestedPartnersOpportunityCommandHandler(
IOpportunityRepository opportunityRepository, ICompanyRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetInterestedPartnersOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository
                .FindByIdAsync(
                    request.Id,
                    queryOptions => queryOptions.Include(o => o.InterestedPartners),
                    cancellationToken);

            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }

            var distinctIds = request.CompanyIds.Distinct().ToList();

            // 2. Load the company profiles we want to associate
            var companies = await _companyProfileRepository.FindAllAsync(
                x => distinctIds.Contains(x.Id),
                cancellationToken);

            opportunity.SetInterestedPartners(companies);
        }
    }
}