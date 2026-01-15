using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Countries.GetCountries
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDto>>
    {
        private readonly ICountryRepository _countryRepository;

        [IntentManaged(Mode.Merge)]
        public GetCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _countryRepository.FindAllProjectToAsync<CountryDto>(cancellationToken);
            return countries;
        }
    }
}