using System.Runtime.CompilerServices;
using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.CreateCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationIdentityUserRepository _applicationIdentityUserRepository;

        [IntentManaged(Mode.Merge)]
        public CreateCompanyCommandHandler(
            ICompanyRepository companyRepository,
            IServiceTypeRepository serviceTypeRepository,
            ICapabilityRepository capabilityRepository,
            IIndustryRepository industryRepository,
            ICurrentUserService currentUserService,
            IApplicationIdentityUserRepository applicationIdentityUserRepository)
        {
            _companyRepository = companyRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _capabilityRepository = capabilityRepository;
            _industryRepository = industryRepository;
            _currentUserService = currentUserService;
            _applicationIdentityUserRepository = applicationIdentityUserRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("Current user is not authenticated.");

            ApplicationIdentityUser? applicationUser = currentUser.Id != null
                ? await _applicationIdentityUserRepository.FindByIdAsync(currentUser.Id.Value.ToString(), cancellationToken)
                : null;

            if (applicationUser == null) throw new NotFoundException("Application user is null.");

            var company = new Company(
                name: request.Name,
                shortDescription: request.ShortDescription,
                fullDescription: request.FullDescription,
                websiteUrl: request.WebsiteUrl,
                employeeCount: request.EmployeeCount);

            var serviceTypes = request.ServiceTypes == null ? new List<ServiceType>() : await _serviceTypeRepository.FindByIdsAsync(request.ServiceTypes.ToArray(), cancellationToken);
            var capabilities = request.Capabilities == null ? new List<Capability>() : await _capabilityRepository.FindByIdsAsync(request.Capabilities.ToArray(), cancellationToken);
            var industries = request.Industries == null ? new List<Industry>() : await _industryRepository.FindByIdsAsync(request.Industries.ToArray(), cancellationToken);

            company.SetServiceTypes(serviceTypes);
            company.SetCapabilities(capabilities);
            company.SetIndustries(industries);
            company.AddUser(applicationUser);

            _companyRepository.Add(company);
            await _companyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return company.Id;
        }
    }
}