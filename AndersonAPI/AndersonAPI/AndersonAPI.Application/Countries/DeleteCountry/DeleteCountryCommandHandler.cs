using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Countries.DeleteCountry
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand>
    {
        private readonly ICountryRepository _countryRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (country is null)
            {
                throw new NotFoundException($"Could not find Country '{request.Id}'");
            }


            _countryRepository.Remove(country);
        }
    }
}