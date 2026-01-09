using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.CompanyProfiles.CreateCompanyProfile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateCompanyProfileCommandHandler : IRequestHandler<CreateCompanyProfileCommand, Guid>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public CreateCompanyProfileCommandHandler(ICompanyProfileRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateCompanyProfileCommand request, CancellationToken cancellationToken)
        {
            var companyProfile = new CompanyProfile(
                name: request.Name,
                shortDescription: request.ShortDescription,
                description: request.Description,
                websiteUrl: request.WebsiteUrl,
                employeeCount: request.EmployeeCount);

            _companyProfileRepository.Add(companyProfile);
            await _companyProfileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return companyProfile.Id;
        }
    }
}