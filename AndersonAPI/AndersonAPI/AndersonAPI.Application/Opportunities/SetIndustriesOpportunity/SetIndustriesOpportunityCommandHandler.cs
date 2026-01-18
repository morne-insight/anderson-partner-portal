using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.SetIndustriesOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetIndustriesOpportunityCommandHandler : IRequestHandler<SetIndustriesOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public SetIndustriesOpportunityCommandHandler(IOpportunityRepository opportunityRepository, IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetIndustriesOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }

            var industries = await _industryRepository.FindByIdsAsync(request.IndustryIds.ToArray(), cancellationToken);
            opportunity.SetIndustries(industries);
        }
    }
}