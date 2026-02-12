using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetApiHealth
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetApiHealthQueryHandler : IRequestHandler<GetApiHealthQuery, string>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<GetApiHealthQueryHandler> _logger;

        [IntentManaged(Mode.Merge)]
        public GetApiHealthQueryHandler(ICountryRepository countryRepository, ILogger<GetApiHealthQueryHandler> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        /// <summary>
        /// Hits the database via CQRS pipeline without authentication
        /// </summary>
        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<string> Handle(GetApiHealthQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("logging with _logger.");
            Log.Information("Logging with Log");
            var count = await _countryRepository.CountAsync();
            return $"API is healthy. Country count: {count}";
        }
    }
}