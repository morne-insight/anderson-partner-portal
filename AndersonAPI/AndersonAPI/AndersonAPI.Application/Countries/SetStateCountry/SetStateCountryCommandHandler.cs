using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Countries.SetStateCountry
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetStateCountryCommandHandler : IRequestHandler<SetStateCountryCommand>
    {
        private readonly ICountryRepository _countryRepository;

        [IntentManaged(Mode.Merge)]
        public SetStateCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SetStateCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (country is null)
            {
                throw new NotFoundException($"Could not find Country '{request.Id}'");
            }

            country.SetState(request.State);
        }
    }
}