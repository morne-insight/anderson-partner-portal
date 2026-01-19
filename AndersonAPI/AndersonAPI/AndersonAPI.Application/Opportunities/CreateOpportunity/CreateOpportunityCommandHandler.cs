using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Opportunities.CreateOpportunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateOpportunityCommandHandler : IRequestHandler<CreateOpportunityCommand, Guid>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public CreateOpportunityCommandHandler(
            IOpportunityRepository opportunityRepository, 
            IServiceTypeRepository serviceTypeRepository,
            ICapabilityRepository capabilityRepository,
            IIndustryRepository industryRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _capabilityRepository = capabilityRepository;
            _industryRepository = industryRepository;
            _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<Guid> Handle(CreateOpportunityCommand request, CancellationToken cancellationToken)
        {
            var opportunity = new Opportunity(
                title: request.Title,
                shortDescription: request.ShortDescription,
                fullDescription: request.FullDescription,
                deadline: request.Deadline,
                opportunityTypeId: request.OpportunityTypeId,
                countryId: request.CountryId,
                companyId: request.CompanyId);

            var serviceTypes = request.ServiceTypes == null ? new List<ServiceType>() : await _serviceTypeRepository.FindByIdsAsync(request.ServiceTypes.ToArray(), cancellationToken);
            var capabilities = request.Capabilities == null ? new List<Capability>() : await _capabilityRepository.FindByIdsAsync(request.Capabilities.ToArray(), cancellationToken);
            var industries = request.Industries == null ? new List<Industry>() : await _industryRepository.FindByIdsAsync(request.Industries.ToArray(), cancellationToken);

            opportunity.SetServiceTypes(serviceTypes);
            opportunity.SetCapabilities(capabilities);
            opportunity.SetIndustries(industries);

            _opportunityRepository.Add(opportunity);
            await _opportunityRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return opportunity.Id;
        }
    }
}