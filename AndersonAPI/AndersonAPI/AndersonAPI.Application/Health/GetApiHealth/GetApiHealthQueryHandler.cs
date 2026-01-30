using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetApiHealth
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetApiHealthQueryHandler : IRequestHandler<GetApiHealthQuery, string>
    {
        private readonly ICountryRepository _countryRepository;

        [IntentManaged(Mode.Merge)]
        public GetApiHealthQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Hits the database via CQRS pipeline without authentication
        /// </summary>
        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<string> Handle(GetApiHealthQuery request, CancellationToken cancellationToken)
        {
            var count = await _countryRepository.CountAsync();
            return $"API is healthy. Country count: {count}";
        }
    }
}