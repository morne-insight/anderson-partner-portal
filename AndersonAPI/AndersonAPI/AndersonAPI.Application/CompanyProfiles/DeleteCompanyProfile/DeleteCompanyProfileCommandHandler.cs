using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.CompanyProfiles.DeleteCompanyProfile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteCompanyProfileCommandHandler : IRequestHandler<DeleteCompanyProfileCommand>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteCompanyProfileCommandHandler(ICompanyProfileRepository companyProfileRepository)
        {
            _companyProfileRepository = companyProfileRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteCompanyProfileCommand request, CancellationToken cancellationToken)
        {
            var companyProfile = await _companyProfileRepository.FindByIdAsync(request.Id, cancellationToken);
            if (companyProfile is null)
            {
                throw new NotFoundException($"Could not find CompanyProfile '{request.Id}'");
            }


            _companyProfileRepository.Remove(companyProfile);
        }
    }
}