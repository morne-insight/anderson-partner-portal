using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.AddInterestedPartnerOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AddInterestedPartnerOpportunityCommandHandler : IRequestHandler<AddInterestedPartnerOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ICompanyRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public AddInterestedPartnerOpportunityCommandHandler(
            IOpportunityRepository opportunityRepository,
            ICompanyRepository companyProfileRepository)
        {
            _opportunityRepository = opportunityRepository;
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(AddInterestedPartnerOpportunityCommand request, CancellationToken cancellationToken)
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

            // 2. Load the company profiles we want to associate
            var company = await _companyProfileRepository.FindByIdAsync(
                request.PartnerId,
                cancellationToken);

            if (company is null) throw new NotFoundException($"Could not find Company Profile '{request.PartnerId}'");

            opportunity.AddInterestedPartner(company);
        }
    }
}