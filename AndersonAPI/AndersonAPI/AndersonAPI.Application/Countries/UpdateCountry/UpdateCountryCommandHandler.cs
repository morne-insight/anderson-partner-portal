using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Countries.UpdateCountry
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand>
    {
        private readonly ICountryRepository _countryRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (country is null)
            {
                throw new NotFoundException($"Could not find Country '{request.Id}'");
            }

            country.Update(request.RegionId, request.Name, request.Description);
        }
    }
}