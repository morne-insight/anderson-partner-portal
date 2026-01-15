using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.CreateOppertunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateOppertunityCommandHandler : IRequestHandler<CreateOppertunityCommand, Guid>
    {
        private readonly IOppertunityRepository _oppertunityRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public CreateOppertunityCommandHandler(
            IOppertunityRepository oppertunityRepository,
            IServiceTypeRepository serviceTypeRepository,
            ICapabilityRepository capabilityRepository,
            IIndustryRepository industryRepository)
        {
            _oppertunityRepository = oppertunityRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _capabilityRepository = capabilityRepository;
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<Guid> Handle(CreateOppertunityCommand request, CancellationToken cancellationToken)
        {

            var oppertunity = new Oppertunity(
                title: request.Title,
                shortDescription: request.ShortDescription,
                fullDescription: request.FullDescription,
                deadline: request.Deadline,
                oppertunityTypeId: request.OppertunityTypeId,
                countryId: request.CountryId,
                companyId: request.CompanyId);

            var serviceTypes = request.ServiceTypes == null ? new List<ServiceType>() : await _serviceTypeRepository.FindByIdsAsync(request.ServiceTypes.ToArray(), cancellationToken);
            var capabilities = request.Capabilities == null ? new List<Capability>() : await _capabilityRepository.FindByIdsAsync(request.Capabilities.ToArray(), cancellationToken);
            var industries = request.Industries == null ? new List<Industry>() : await _industryRepository.FindByIdsAsync(request.Industries.ToArray(), cancellationToken);

            oppertunity.SetServiceTypes(serviceTypes);
            oppertunity.SetCapabilities(capabilities);
            oppertunity.SetIndustries(industries);

            _oppertunityRepository.Add(oppertunity);
            await _oppertunityRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return oppertunity.Id;
        }
    }
}