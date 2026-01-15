using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Countries.GetCountryById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDto>
    {
        private readonly ICountryRepository _countryRepository;

        [IntentManaged(Mode.Merge)]
        public GetCountryByIdQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.FindByIdProjectToAsync<CountryDto>(request.Id, cancellationToken);
            if (country is null)
            {
                throw new NotFoundException($"Could not find Country '{request.Id}'");
            }
            return country;
        }
    }
}