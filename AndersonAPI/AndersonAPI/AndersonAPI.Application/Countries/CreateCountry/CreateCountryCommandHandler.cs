using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Countries.CreateCountry
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Guid>
    {
        private readonly ICountryRepository _countryRepository;

        [IntentManaged(Mode.Merge)]
        public CreateCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var country = new Country(
                regionId: request.RegionId,
                name: request.Name,
                description: request.Description);

            _countryRepository.Add(country);
            await _countryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return country.Id;
        }
    }
}