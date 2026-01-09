using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.CompanyProfiles.UpdateProfileCompanyProfile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateProfileCompanyProfileCommandHandler : IRequestHandler<UpdateProfileCompanyProfileCommand>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateProfileCompanyProfileCommandHandler(ICompanyProfileRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateProfileCompanyProfileCommand request, CancellationToken cancellationToken)
        {
            var companyProfile = await _companyProfileRepository.FindByIdAsync(request.Id, cancellationToken);
            if (companyProfile is null)
            {
                throw new NotFoundException($"Could not find CompanyProfile '{request.Id}'");
            }

            companyProfile.UpdateProfile(
                request.Name,
                request.ShortDescription,
                request.Description,
                request.WebsiteUrl,
                request.EmployeeCount);
        }
    }
}