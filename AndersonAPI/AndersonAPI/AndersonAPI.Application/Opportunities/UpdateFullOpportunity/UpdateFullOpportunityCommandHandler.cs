using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.UpdateFullOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateFullOpportunityCommandHandler : IRequestHandler<UpdateFullOpportunityCommand>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateFullOpportunityCommandHandler(
            IOpportunityRepository opportunityRepository,
            IServiceTypeRepository serviceTypeRepository,
            ICapabilityRepository capabilityRepository,
            IIndustryRepository industryRepository)
        {
            _opportunityRepository = opportunityRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _capabilityRepository = capabilityRepository;
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(UpdateFullOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository
                .FindByIdAsync(request.Id,
                    queryOptions => queryOptions
                        .Include(q => q.Capabilities)
                        .Include(q => q.Industries)
                        .Include(q => q.ServiceTypes),
                    cancellationToken);

            if (opportunity is null)
            {
                throw new NotFoundException($"Could not find Opportunity '{request.Id}'");
            }

            opportunity.UpdateFull(
                request.Title,
                request.ShortDescription,
                request.FullDescription,
                request.Deadline,
                request.OpportunityTypeId,
                request.CountryId,
                request.ServiceTypes,
                request.Capabilities,
                request.Industries);

            var serviceTypes = request.ServiceTypes == null ? new List<ServiceType>() : await _serviceTypeRepository.FindByIdsAsync(request.ServiceTypes.ToArray(), cancellationToken);
            var capabilities = request.Capabilities == null ? new List<Capability>() : await _capabilityRepository.FindByIdsAsync(request.Capabilities.ToArray(), cancellationToken);
            var industries = request.Industries == null ? new List<Industry>() : await _industryRepository.FindByIdsAsync(request.Industries.ToArray(), cancellationToken);

            opportunity.SetServiceTypes(serviceTypes);
            opportunity.SetCapabilities(capabilities);
            opportunity.SetIndustries(industries);

        }
    }
}