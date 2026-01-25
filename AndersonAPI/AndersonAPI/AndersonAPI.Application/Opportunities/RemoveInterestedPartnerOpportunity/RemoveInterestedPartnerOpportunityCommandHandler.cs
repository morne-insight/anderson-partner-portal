using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.RemoveInterestedPartnerOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class RemoveInterestedPartnerOpportunityCommandHandler : IRequestHandler<RemoveInterestedPartnerOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public RemoveInterestedPartnerOpportunityCommandHandler(
            IOpportunityRepository opportunityRepository,
            ICompanyRepository companyRepository)
        {
            _opportunityRepository = opportunityRepository;
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(RemoveInterestedPartnerOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository
                .FindByIdAsync(
                    request.Id,
                    queryOptions => queryOptions.Include(o => o.InterestedPartners),
                    cancellationToken);

            if (opportunity is null) throw new NotFoundException($"Could not find Opportunity '{request.Id}'");

            opportunity.RemoveInterestedPartner(request.PartnerId);
        }
    }
}