using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.SetSubmittedQuarterly
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetSubmittedQuarterlyCommandHandler : IRequestHandler<SetSubmittedQuarterlyCommand>
    {
        private readonly IQuarterlyRepository _quarterlyRepository;

        [IntentManaged(Mode.Merge)]
        public SetSubmittedQuarterlyCommandHandler(IQuarterlyRepository quarterlyRepository)
        {
            _quarterlyRepository = quarterlyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(SetSubmittedQuarterlyCommand request, CancellationToken cancellationToken)
        {
            var quarterly = await _quarterlyRepository.FindByIdAsync(request.Id, cancellationToken);
            if (quarterly is null)
            {
                throw new NotFoundException($"Could not find Quarterly '{request.Id}'");
            }

            quarterly.SetSubmitted(request.IsSubmitted);
        }
    }
}